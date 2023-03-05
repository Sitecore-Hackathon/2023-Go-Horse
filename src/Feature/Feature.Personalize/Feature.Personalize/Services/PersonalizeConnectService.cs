using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using Sitecore.Resources.Media;
using Sitecore.Rules.ConditionalRenderings;
using Sitecore.Sites;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI.WebControls;

namespace Feature.Personalize.Services
{
    public class PersonalizeConnectService : IPersonalizeConnectService
    {
        /// <summary>
        /// Get Item from a given URL
        /// </summary>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        public Item GetItemFromPageUrl(string pageUrl)
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
            return siteContext.Database.GetItem(fullPath);
        }

        /// <summary>
        /// Executes the PersonalizeConnect Conditions and returns the Datasource that matches the experienceId and experienceValue
        /// </summary>
        /// <param name="item">Item to be tested</param>
        /// <param name="experienceId">Personalize Experience ID</param>
        /// <param name="experienceValue">Personalize Experience Value</param>
        /// <param name="renderingReference">Rendering to be processed</param>
        /// <returns></returns>
        public Item GetDatasourceFromRules(Item item, string experienceId, string experienceValue, RenderingReference renderingReference)
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

        /// <summary>
        /// Determine if a given rendering uses a PersonalizeConnect Condition
        /// </summary>
        /// <param name="renderingRef"></param>
        /// <returns></returns>
        public bool HasPersonalizeCondition(RenderingReference renderingRef)
        {
            var ret = false;
            foreach (var rule in renderingRef.Settings.Rules.Rules)
            {
                var property = rule.Condition.GetType().GetProperty("ConditionItemId", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                if (property == null)
                    continue;
                var value = (ID)property.GetValue(rule.Condition);
                if (value.ToString() == Constants.PersonalizeConnectConditionID)
                {
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Get JSON to represent a LinkField
        /// </summary>
        /// <param name="linkField"></param>
        /// <returns></returns>
        public dynamic GetLinkField(LinkField linkField)
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

            return new
            {
                href = url,
                target = linkField.Target,
                text = linkField.Text,
                title = linkField.Title,
                @class = linkField.Class
            };
        }

        /// <summary>
        /// Get JSON to represent a ImageField
        /// </summary>
        /// <param name="imageField"></param>
        /// <returns></returns>
        public dynamic GetImageField(Sitecore.Data.Fields.ImageField imageField)
        {
            string src = "";
            string alt = "";
            if (imageField != null && imageField.MediaItem != null)
            {
                var imageItem = new MediaItem(imageField.MediaItem);
                src = StringUtil.EnsurePrefix('/', MediaManager.GetMediaUrl(imageItem));
                alt = imageItem.Alt;
            }
            return new
            {
                src,
                alt
            };
        }
    }
}