using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace team3
{
    class Message
    {
        public static readonly string USER_LOGIN_SUCESSFULLY = "登入成功";
        public static readonly string USER_LOGIN_FAIL = "登入失敗";

        public static readonly string USER_REGISTER_ACCOUNT_LENGTH_ERROR = "請確認帳號長度為6個字元以上，40個字元以內";
        public static readonly string USER_REGISTER_ACCOUNT_FORMAT_ERROR = "請確認帳號是由英文字、數字、底線組成";
        public static readonly string USER_REGISTER_ACCOUNT_EXISTS = "此帳號已註冊過";
        public static readonly string USER_REGISTER_PASSWORD_LENGTH_ERROR = "請確認帳號長度為6個字元以上，40個字元以內";
        public static readonly string USER_REGISTER_EMAIL_FORMAT_ERROR = "Email 格式錯誤";
        public static readonly string USER_REGISTER_EMAIL_EXISTS_ERROR = "此Email 已註冊過";
        public static readonly string USER_REGISTER_PASSWORD_CONFIRM_ERROR = "密碼確認不一致";
        public static readonly string USER_REGISTER_SUCCESSFULLY = "註冊成功";
        public static readonly string USER_REGISTER_FAIL = "註冊失敗";

        public static readonly string USER_EDIT_EMAIL_FORMAT_ERROR = "Email 格式錯誤";
        public static readonly string USER_EDIT_EMAIL_EXISTS_ERROR = "此Email 已註冊過";
        public static readonly string USER_EDIT_EMAIL_SUCCESSFULLY = "修改Email成功";
        public static readonly string USER_EDIT_EMAIL_FAIL = "修改Email失敗";
        

    }
    
    class StringUtil
    {
        public static bool isValidEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return true;
            else
                return false;
        }

        public static bool isVaildAccountFormat(string inputAccount)
        {
            string strRegex = @"[A-Za-z0-9_]{6}";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputAccount))
                return true;
            else
                return false;
        }
    }
}
