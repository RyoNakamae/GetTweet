using CoreTweet;
using CoreTweet.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace GetTweet
{
    class Tweet
    {
        Tokens tokens;

        public Tweet()
        {
            #region 認証
            var api_key = ConfigurationManager.AppSettings["api_key"];
            var api_secret = ConfigurationManager.AppSettings["api_secret"];
            var token = ConfigurationManager.AppSettings["token"];
            var token_secret = ConfigurationManager.AppSettings["token_secret"];

            tokens = CoreTweet.Tokens.Create(api_key, api_secret, token, token_secret);
            #endregion
        }


        /// <summary>
        /// 自分のツィートの中でファボられたものを取得する
        /// </summary>
        public void GetFavo()
        {
            var parm = new Dictionary<string, object>();
            parm["count"] = 10;
            parm["include_rts"] = false;
            
            //自分のツィートを取得
            var tweets = tokens.Statuses.UserTimeline(parm);

            var list = new List<Status>();
            foreach (var tweet in tweets)
            {
                //tweet.FavoriteCountが1以上のものがファぼられたもの
                if (tweet.FavoriteCount > 0)
                {
                    list.Add(tweet);
                }
            }

            //所定のファイルに追記する
            using (StreamWriter sw = new StreamWriter(ConfigurationManager.AppSettings["OutFilePath"], true, Encoding.UTF8))
            {
                foreach (var tweet in list)
                {
                    writeTweet(sw, tweet);
                }
            }
        }

        public List<string> GetTrend()
        {
            var parm = new Dictionary<string, object>();

            parm["id"] = 23424856;
            var t = tokens.Trends.Place(parm);

            var resList = new List<string>();
            foreach (var trend in t[0].Trends)
            {
                resList.Add(trend.Name);
            }

            return resList;
        }

        public void GetByUser(string screen_name)
        {
            try {
                #region ユーザ指定で取得
                var parm = new Dictionary<string, object>();
                parm["count"] = 10;
                parm["screen_name"] = screen_name;

                var tweets = tokens.Statuses.UserTimeline(parm);
                #endregion

                //所定のファイルに追記する
                using (StreamWriter sw = new StreamWriter(ConfigurationManager.AppSettings["OutFilePath"], true, Encoding.UTF8))
                {
                    foreach (var tweet in tweets)
                    {
                        writeTweet(sw, tweet);
                    }
                }
            }
            catch { }
        }

        public void GetByKeyword(string keyword,int cnt)
        {
            try {
                var tweets = tokens.Search.Tweets(count => cnt, q => keyword);

                //所定のファイルに追記する
                using (StreamWriter sw = new StreamWriter(ConfigurationManager.AppSettings["OutFilePath"], true, Encoding.UTF8))
                {
                    foreach (var tweet in tweets)
                    {
                        writeTweet(sw, tweet);
                    }
                }
            }
            catch { }
        }

        void writeTweet(StreamWriter sw, Status tweet)
        {
            #region RT,@のものは除外
            if (tweet.IsRetweeted != null && (bool)tweet.IsRetweeted) return;

            if (tweet.Text.Contains("@")) return;
            if (tweet.Text.ToUpper().Contains("RT")) return;
            #endregion

            Console.WriteLine("{0}: {1}", tweet.User.ScreenName, tweet.Text);
            Console.WriteLine("---------");
            sw.WriteLine(replace(tweet.Text));

        }

        string replace(string text)
        {
            var res = text;

            //ハッシュタグ以降をのぞく
            if (res.Contains("#"))
            {
                res = res.Remove(res.IndexOf('#'));
            }

            //リンク以降を除く
            if (res.Contains("https"))
            {
                res = res.Remove(res.IndexOf("https"));
            }

            return res;
        }
    }
}
