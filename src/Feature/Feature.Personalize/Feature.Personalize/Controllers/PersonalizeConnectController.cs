using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using System.Linq;
using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Fields;
using System.Web.UI.WebControls;
using Feature.Personalize.Services;

namespace Feature.Personalize.Controllers
{
    /// <summary>
    /// PersonalizeConnect Controller
    /// </summary>
    public class PersonalizeConnectController : Controller
    {
        private readonly IPersonalizeConnectService _service;
        public PersonalizeConnectController(IPersonalizeConnectService service)
        {
            _service = service;
        }

        /// <summary>
        /// PersonalizeConnect Action that returns a JSON for a given experienceId, experienceValue and pageUrl
        /// </summary>
        /// <param name="experienceId"></param>
        /// <param name="experienceValue"></param>
        /// <param name="pageUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Index(string experienceId, string experienceValue, string pageUrl)
        {
            // Get PageItem
            var pageItem = _service.GetItemFromPageUrl(pageUrl);
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
                if (!_service.HasPersonalizeCondition(renderingRef))
                    continue;
                datasource = _service.GetDatasourceFromRules(pageItem, experienceId, experienceValue, renderingRef);
                if (datasource != null)
                {
                    // Get default datasource ID to be used as container
                    defaultDatasource = _service.GetDatasourceFromRules(pageItem, experienceId, new ID().ToString(), renderingRef);
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
                fields
            };

            foreach (Field field in datasource.Fields.Cast<Field>())
            {
                if (fields.ContainsKey(field.Key))
                    continue;

                var type = FieldTypeManager.GetField(field);

                if (type is Sitecore.Data.Fields.ImageField)
                {
                    var imgData = _service.GetImageField((Sitecore.Data.Fields.ImageField)field);
                    fields.Add(field.Key, imgData);
                }
                else if (type is LinkField)
                {
                    var linkData = _service.GetLinkField((LinkField)type);
                    fields.Add(field.Key, linkData);
                }
                else
                    fields.Add(field.Key, field.Value);
            }

            return Json(ret, JsonRequestBehavior.AllowGet);
        }
    }
}