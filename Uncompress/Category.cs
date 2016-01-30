namespace NthsKeys.Uncompresser
{
    public class Category
    {
        private static string[] categories = new string[] {
            "语文",
            "数学",
            "英语",
            "物理必修",
            "化学必修",
            "历史必修",
            "政治必修",
            "地理必修",
            "生物必修",
            "物理选修",
            "化学选修",
            "历史选修",
            "政治选修",
            "地理选修",
            "生物选修",
            "物理",
            "化学",
            "历史",
            "政治",
            "地理",
            "生物",
        };

        public static string MatchCategory(string phrase)
        {
            foreach (string category in categories)
            {
                if (phrase.Contains(category))
                {
                    return category;
                }
            }
            return "杂项";
        }
    }
}
