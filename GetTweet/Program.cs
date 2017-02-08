using System.Configuration;

namespace GetTweet
{
    class Program
    {
        static void Main(string[] args)
        {
            var tweet = new Tweet();

            #region ファボられたものを取得する
            tweet.GetFavo();
            #endregion

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

            #region トレンドを取得
            var trendList = tweet.GetTrend();
            foreach (var keyword in trendList)
            {
                tweet.GetByKeyword(keyword, 5);
            }
            #endregion

            #region キーワード指定で取得
            var keywords = ConfigurationManager.AppSettings["keywords"];

            if (!string.IsNullOrEmpty(keywords))
            {
                foreach (var keyword in keywords.Split(','))
                {
                    tweet.GetByKeyword(keyword, 10);
                }
            }
            #endregion
        }
    }
}
