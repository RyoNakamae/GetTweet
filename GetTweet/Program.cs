using System.Configuration;

namespace GetTweet
{
    class Program
    {
        static void Main(string[] args)
        {
            var tweet = new Tweet();

            #region ユーザ指定で取得
            var screenNames = ConfigurationManager.AppSettings["screen_names"];

            if (!string.IsNullOrEmpty(screenNames))
            {
                foreach (var user in screenNames.Split(','))
                {
                    tweet.GetByUser(user);
                }
            }
            #endregion

            #region キーワード指定で取得
            var keywords = ConfigurationManager.AppSettings["keywords"];

            if (!string.IsNullOrEmpty(keywords))
            {
                foreach (var keyword in keywords.Split(','))
                {
                    tweet.GetByKeyword(keyword);
                }
            }
            #endregion
        }
    }
}
