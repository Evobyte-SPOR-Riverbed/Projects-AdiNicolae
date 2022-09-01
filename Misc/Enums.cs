using System.ComponentModel.DataAnnotations;

namespace Drinktionary.Misc;

public class Enums
{
    public enum DrinkerType
    {
        [Display(Name = "Social Drinker")]
        SocialDrinker,

        [Display(Name = "Conformity Drinker")]
        ConformityDrinker,

        [Display(Name = "Enhancement Drinker")]
        EnhancementDrinker,

        [Display(Name = "Coping Drinker")]
        CopingDrinker,

        [Display(Name = "None")]
        None
    }

    public enum SexType
    {
        Male,
        Female,
        Intersex,
        Other
    }
}