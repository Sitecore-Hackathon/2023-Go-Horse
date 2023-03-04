using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Feature.Personalize.Rules
{
    public class PersonalizeConnectCondition<T> : StringOperatorCondition<T> where T : RuleContext
    {
        public string experienceId { get; set; }
        public string comparesTo { get; set; }
        public string experienceValue { get; set; }

        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");
            OperatorId = comparesTo;
            string currentExperienceId = ruleContext.Parameters["experienceId"]?.ToString();
            string currentExperienceValue = ruleContext.Parameters["experienceValue"]?.ToString();
            return Compare(currentExperienceId, experienceId) && Compare(currentExperienceValue, experienceValue);
        }
    }
}