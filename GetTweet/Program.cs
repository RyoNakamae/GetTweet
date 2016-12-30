using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTweet
{
    class Program
    {
        static void Main(string[] args)
        {

            #region 認証
            var api_key = ConfigurationManager.AppSettings["api_key"];
            var api_secret = ConfigurationManager.AppSettings["api_secret"];
            var token = ConfigurationManager.AppSettings["token"];
            var token_secret = ConfigurationManager.AppSettings["token_secret"];

            var tokens = CoreTweet.Tokens.Create(api_key, api_secret, token, token_secret);
            #endregion

            #region ユーザ指定で取得
            var screenName = ConfigurationManager.AppSettings["screen_name"];

            var parm = new Dictionary<string, object>();
            parm["count"] = 100;
            parm["screen_name"] = screenName;

            var tweets = tokens.Statuses.UserTimeline(parm);

            //所定のファイルに追記する
            using (StreamWriter sw = new StreamWriter(ConfigurationManager.AppSettings["OutFilePath"], true, Encoding.UTF8))
            {
                foreach (var tweet in tweets)
                {
                    Console.WriteLine("{0}: {1}",tweet.User.ScreenName,tweet.Text);
                    Console.WriteLine("---------");
                    sw.WriteLine(tweet.Text);
                }
            }
            #endregion

        }
    }
}
