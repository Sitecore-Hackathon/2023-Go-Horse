using Sitecore.Sites;
using Sitecore;
using System;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using System.Linq;
using System.Collections.Generic;
using Sitecore.Rules.ConditionalRenderings;
using Sitecore.Rules;
using Sitecore.Data;
using System.Reflection;
using Sitecore.Data.Fields;
using System.Web.UI.WebControls;
using Sitecore.Resources.Media;

namespace Feature.Personalize.Controllers
{
    public class PersonalizeConnectController : Controller
    {
        private static readonly string personalizeConnectConditionID = "{8DCAA413-D590-4B02-B471-66E62ED57A0D}";

        [HttpGet]
        public JsonResult Index(string experienceId, string experienceValue, string pageUrl)
        {
            // Get PageItem
            var pageItem = GetItemFromPageUrl(pageUrl);
            if (pageItem == null)
                return Json(new { result = "error", error = $"Page item not found for url {pageUrl}" }, JsonRequestBehavior.AllowGet);

            // Get all Renderings with Personalization
            var renderings = pageItem.Visualization.GetRenderings(Sitecore.Context.Device, false);
            var renderingsWithRules = renderings.Where(x => x.Settings.Rules.Count > 0).ToList();

            // Get Datasource according to the Rules
            Item datasource = null;
            Item defaultDatasource = null;
            foreach (RenderingReference renderingRef in renderingsWithRules)
            {
                if (!HasPersonalizeCondition(renderingRef))
                    continue;
                datasource = GetDatasourceFromRules(pageItem, experienceId, experienceValue, renderingRef);
                if (datasource != null)
                {
                    // Get default datasource ID to be used as container
                    defaultDatasource = GetDatasourceFromRules(pageItem, experienceId, (new ID()).ToString(), renderingRef);
                    break;
                }
            }
            if (datasource == null)
                return Json(new { result = "error", error = $"Datasource not found for url {pageUrl} - experienceId: {experienceId} - experienceValue: {experienceValue}" }, JsonRequestBehavior.AllowGet);

            // Output the Datasource Item as Json
            var fields = new Dictionary<string, dynamic>();
            dynamic ret = new
            {
                id = datasource.ID.ToString(),
                path = datasource.Paths.Path,
                name = datasource.Name,
                displayName = datasource.DisplayName,
                version = datasource.Version.ToString(),
                language = datasource.Language.Name,
                container = defaultDatasource == null ? "" : defaultDatasource.ID.ToString(),
                fields = fields
            };

            foreach (Field field in datasource.Fields)
            {
                if (fields.ContainsKey(field.Key))
                    continue;

                var type = FieldTypeManager.GetField(field);

                if (type is Sitecore.Data.Fields.ImageField)
                {
                    var imageField = (Sitecore.Data.Fields.ImageField)field;
                    string src = "";
                    string alt = "";
                    if (imageField != null && imageField.MediaItem != null)
                    {
                        var imageItem = new MediaItem(imageField.MediaItem);
                        src = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(imageItem));
                        alt = imageItem.Alt;
                    }
                    dynamic img = new
                    {
                        src = src,
                        alt = alt
                    };
                    fields.Add(field.Key, img);
                }
                else if (type is LinkField)
                {
                    var linkData = GetLinkField((LinkField)type);
                    fields.Add(field.Key, linkData);
                }
                else
                    fields.Add(field.Key, field.Value);
            }

            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        private dynamic GetLinkField(LinkField linkField)
        {
            string url = String.Empty;

            switch (linkField.LinkType)
            {
                case "internal":
                case "external":
                case "mailto":
                case "anchor":
                case "javascript":
                    url = linkField.Url;
                    break;
                case "media":
                    MediaItem media = new MediaItem(linkField.TargetItem);
                    url = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(media));
                    break;
                default:
                    break;
            }

            dynamic linkData = new
            {
                href = url,
                target = linkField.Target,
                text = linkField.Text,
                title = linkField.Title,
                @class = linkField.Class
            };
            return linkData;
        }

        private bool HasPersonalizeCondition(RenderingReference renderingRef)
        {
            var ret = false;
            foreach (var rule in renderingRef.Settings.Rules.Rules)
            {
                var property = rule.Condition.GetType().GetProperty("ConditionItemId", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                if (property == null)
                    continue;
                var value = (ID)property.GetValue(rule.Condition);
                if (value.ToString() == personalizeConnectConditionID)
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        private Item GetDatasourceFromRules(Item item, string experienceId, string experienceValue, RenderingReference renderingReference)
        {
            // Instantiates renderingReference, from there obtain the rules
            var rules = renderingReference.Settings.Rules;

            // No rules? Returns null
            if (rules.Count == 0)
                return null;

            // Instantiates the ruleContext to run the rule
            var renderingsRuleContext =
                new ConditionalRenderingsRuleContext(new List<RenderingReference> { renderingReference },
                    renderingReference)
                { Item = item };
            renderingsRuleContext.Parameters["experienceId"] = experienceId;
            renderingsRuleContext.Parameters["experienceValue"] = experienceValue;
            rules.RunFirstMatching(renderingsRuleContext);

            var newReference = renderingsRuleContext.References.Find(r1 => r1.UniqueId == renderingsRuleContext.Reference.UniqueId);
            var dataSource = newReference.Settings.DataSource;
            if (string.IsNullOrEmpty(dataSource))
                return null;
            var obj = newReference.Database.GetItem(dataSource);
            return (obj != null) ? obj : null;
        }

        private void RulesEvaluatedHandler(RuleList<ConditionalRenderingsRuleContext> ruleList, ConditionalRenderingsRuleContext ruleContext, Rule<ConditionalRenderingsRuleContext> rule)
        {
            if (!ruleContext.IsTesting)
                return;
            Item testDefinitionItem = ruleContext.Item.Database.GetItem(ruleContext.TestId);
            if (testDefinitionItem != null)
                return;
            ruleContext.SkipRule = true;
        }

        private Item GetItemFromPageUrl(string pageUrl)
        {
            var url = new Uri(pageUrl);
            var siteContext = SiteContextFactory.GetSiteContext(url.Host, url.PathAndQuery);

            // Get the path to the Home item
            var homePath = siteContext.StartPath;
            if (!homePath.EndsWith("/"))
                homePath += "/";

            // Get the path to the item, removing virtual path if any
            var itemPath = MainUtil.DecodeName(url.AbsolutePath);
            if (itemPath.StartsWith(siteContext.VirtualFolder))
                itemPath = itemPath.Remove(0, siteContext.VirtualFolder.Length);

            // Obtain the item
            var fullPath = homePath + itemPath;
            var item = siteContext.Database.GetItem(fullPath);
            return item;
        }
    }
}