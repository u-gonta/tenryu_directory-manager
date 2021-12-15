using System;

namespace Shared
{
    namespace Tool
    {
        /// <summary>
        /// オプションの工具クラス
        /// </summary>
        class ClassOption
        {
            /// <summary>
            /// オプション名の可否を確認
            /// </summary>
            /// <param name="name">オプション名</param>
            static private void IsName(string name)
            {
                // オプション名を確認
                if (name.Length <= 0)
                {
                    // オプション名がない
                    throw new Exception("装置番号が設定されていません。");
                }

                // オプション名を確認
                if (name.Length < 9)
                {
                    // オプション名が不適切
                    throw new Exception("装置番号が不適切です。");
                }
            }

            /// <summary>
            /// オプションの番号を取得
            /// </summary>
            /// <param name="name">オプション名</param>
            /// <returns>オプションの番号</returns>
            static public int Number(string name)
            {
                int ret = 0;

                // オプション名の可否を確認
                IsName(name);

                // オプションの番号を数値に変換
                ret = int.Parse(name.Substring(4, 2));

                return ret;
            }

            /// <summary>
            /// バリエーションの番号を取得
            /// </summary>
            /// <param name="name">オプション名</param>
            /// <returns>バリエーションの番号</returns>
            static public int Variation(string name)
            {
                int ret = 0;

                // オプション名の可否を確認
                IsName(name);

                // バリエーションの番号を数値に変換
                ret = int.Parse(name.Substring(6, 2));

                return ret;
            }

            /// <summary>
            /// リビジョンの番号を取得
            /// </summary>
            /// <param name="name">オプション名</param>
            /// <returns>リビジョンの番号</returns>
            static public int Revision(string name)
            {
                int ret = 0;

                // オプション名の可否を確認
                IsName(name);

                // リビジョンの番号を数値に変換
                ret = int.Parse(name.Substring(8, 1));

                return ret;
            }

            /// <summary>
            /// リビジョンの番号に対応した装置名を取得
            /// </summary>
            /// <param name="name">オプション名</param>
            /// <param name="revision">リビジョンの番号</param>
            /// <returns>リビジョンの番号に対応したオプション名</returns>
            static public string Rename(string name, int revision)
            {
                string ret = "";

                // オプション名の可否を確認
                IsName(name);

                // リビジョンの番号を削除
                ret = name.Remove(8, 1);

                // リビジョンの番号を付加
                return ret.Insert(8, revision.ToString("D1"));
            }
        }
    }
}
