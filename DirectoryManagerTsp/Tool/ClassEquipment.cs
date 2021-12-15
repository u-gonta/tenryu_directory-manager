using System;

namespace DirectoryManagerTsp
{
    namespace Tool
    {
        /// <summary>
        /// 装置の工具クラス
        /// </summary>
        class ClassEquipment
        {
            /// <summary>
            /// 装置名の可否を確認
            /// </summary>
            /// <param name="name">装置名</param>
            static private void IsName(string name)
            {
                // 装置名を確認
                if (name.Length <= 0)
                {
                    // 装置名がない
                    throw new Exception("装置番号が設定されていません。");
                }

                // 装置名を確認
                if (name.Length < 9)
                {
                    // 装置名が不適切
                    throw new Exception("装置番号が不適切です。");
                }
            }

            /// <summary>
            /// 装置名のヘッダを取得
            /// </summary>
            /// <param name="model">機種名</param>
            /// <returns>装置名のヘッダ</returns>
            static public string Header(string model)
            {
                string ret = "";

                // 装置名のヘッダに機種名からヘッダと末尾を削除して3桁の0埋めしたを付加
                ret = Setting.ClassIntegration.EquipmentHeader + model.Remove(model.Length - 1).Replace(Setting.ClassIntegration.ModelHeader, "").PadLeft(3, '0');

                return ret;
            }

            /// <summary>
            /// 装置の番号を取得
            /// </summary>
            /// <param name="name">装置名</param>
            /// <returns>装置の番号</returns>
            static public int Number(string name)
            {
                int ret = 0;

                // 装置名の可否を確認
                IsName(name);

                // 装置の番号を数値に変換
                ret = int.Parse(name.Substring(5, 2));

                return ret;
            }

            /// <summary>
            /// リビジョンの番号を取得
            /// </summary>
            /// <param name="name">装置名</param>
            /// <returns>リビジョンの番号</returns>
            static public int Revision(string name)
            {
                int ret = 0;

                // 装置名の可否を確認
                IsName(name);

                // リビジョンの番号を数値に変換
                ret = int.Parse(name.Substring(7, 2));

                return ret;
            }

            /// <summary>
            /// リビジョンの番号に対応した装置名を取得
            /// </summary>
            /// <param name="name">装置名</param>
            /// <param name="revision">リビジョンの番号</param>
            /// <returns>リビジョンの番号に対応した装置名</returns>
            static public string Rename(string name, int revision)
            {
                string ret = "";

                // 装置名の可否を確認
                IsName(name);

                // リビジョンの番号を削除
                ret = name.Remove(7, 2);

                // リビジョンの番号を付加
                return ret.Insert(7, revision.ToString("D2"));
            }
        }
    }
}
