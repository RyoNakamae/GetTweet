using CoreTweet;
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
                        Console.WriteLine("{0}: {1}", tweet.User.ScreenName, tweet.Text);
                        Console.WriteLine("---------");
                        sw.WriteLine(replace(tweet.Text));
                    }
                }
            }
            catch { }
        }

        public void GetByKeyword(string keyword)
        {
            try {
                var tweets = tokens.Search.Tweets(count => 10, q => keyword);

                //所定のファイルに追記する
                using (StreamWriter sw = new StreamWriter(ConfigurationManager.AppSettings["OutFilePath"], true, Encoding.UTF8))
                {
                    foreach (var tweet in tweets)
                    {
                        Console.WriteLine("{0}: {1}", tweet.User.ScreenName, tweet.Text);
                        Console.WriteLine("---------");
                        sw.WriteLine(replace(tweet.Text));
                    }
                }
            }
            catch { }
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
