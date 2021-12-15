using System.IO;

namespace Shared
{
    namespace Tool
    {
        /// <summary>
        /// パスの処理をするクラス
        /// </summary>
        public class ClassPath
        {
            /// <summary>
            /// ディレクトリの区切りを付加
            /// </summary>
            /// <param name="value">ディレクトリのパス</param>
            /// <returns>ディレクトリのパス</returns>
            static public string DirectoryDelimiter(string value)
            {
                string ret = value;

                // パスの終端を確認
                if (ret.EndsWith(Path.DirectorySeparatorChar.ToString()) == false)
                {
                    // 終端がパスの区切りではない ⇒ パスの区切りを付加
                    ret += Path.DirectorySeparatorChar;
                }

                return ret;
            }
        }
    }
}
