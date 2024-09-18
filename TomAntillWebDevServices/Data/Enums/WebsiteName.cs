namespace TomAntillWebDevServices.Data.Enums
{

    public class Website
    {
        public static readonly Website CoatesCarpentry = new("Coates Carpentry"
            , nameof(CoatesCarpentry)
            , new string[] { "BespokeCarpentry", "ConcreteTops" }
            , new string[] { "TregonwellRoad", "PonsfordRoad" });

        public static readonly Website TidyElectrics = new("Tidy Electrics"
            , nameof(TidyElectrics)
            , new string[] { "Lighting", "Refurb" }
            , new string[0]);

        public static readonly Website Portfolio = new("Portfolio"
           , nameof(Portfolio)
           , new string[0]
           , new string[0]);

        public static readonly Website LeahSLT = new("Leah SLT"
           , nameof(LeahSLT)
           , new string[0]
           , new string[0]);

        public static readonly Website Unset = new("Unset"
           , nameof(Unset)
           , new string[0]
           , new string[0]);

        public Website(string name
            , string code
            , string[] uploadCategories
            , string[] projectNames)
        {
            Name = name;
            Code = code;
            UploadCategories = uploadCategories;
            ProjectNames = projectNames;
        }

        public string Name { get; set; }
        public string Code { get; set; }
        public string[] UploadCategories { get; set; }
        public string[] ProjectNames { get; set; }
    }
}
