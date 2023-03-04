using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Layouts;

namespace Feature.Personalize.Services
{
    public interface IPersonalizeConnectService
    {
        dynamic GetLinkField(LinkField linkField);
        dynamic GetImageField(ImageField linkField);
        bool HasPersonalizeCondition(RenderingReference renderingRef);
        Item GetDatasourceFromRules(Item item, string experienceId, string experienceValue, RenderingReference renderingReference);
        Item GetItemFromPageUrl(string pageUrl);
    }
}
