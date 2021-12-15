using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DirectoryManagerTspOption
{
    namespace Status
    {
        /// <summary>
        /// ステータスを管理するクラス
        /// ※シングルトンのテンプレート用クラスから継承
        /// </summary>
        public class ClassManager : Shared.Singleton.ClassTemplate<ClassManager>
        {
            /// <summary>
            /// オプションの情報を保持するクラスをロックするためのインスタンス
            /// </summary>
            private static object _asyncHierarchy = new object();

            /// <summary>
            /// オプションの階層を保持するクラス
            /// </summary>
            private ClassHierarchy _hierarchy = new ClassHierarchy();

            /// <summary>
            /// オプションの階層を保持するクラスを取得
            /// </summary>
            /// <returns>オプションの階層を保持するクラス</returns>
            public ClassHierarchy GetHierarchy()
            {
                var ret = new ClassHierarchy();

                // 排他制御
                lock (_asyncHierarchy)
                {
                    // オプションの階層を保持するクラスをコピー
                    ret = _hierarchy.Clone();
                }

                return ret;
            }

            /// <summary>
            /// オプションの階層を保持するクラスを更新
            /// </summary>
            /// <param name="value">オプションの階層を保持するクラス</param>
            public void SetHierarchy(ClassHierarchy value)
            {
                // 排他制御
                lock (_asyncHierarchy)
                {
                    // オプションの階層を保持するクラスをコピー
                    _hierarchy = value.Clone();
                }
            }

            /// <summary>
            /// オプションの階層を抽出
            /// </summary>
            /// <param name="root">ルートのディレクトリ</param>
            public void ExtractHierarchy(string root)
            {
                // 排他制御
                lock (_asyncHierarchy)
                {
                    // クリア
                    _hierarchy.Clear();

                    // オプション名のディレクトリを探索
                    var extractOptions = new List<string>(Directory.GetDirectories(root, Shared.Setting.ClassOption.Header + "*", SearchOption.TopDirectoryOnly));

                    // オプション名をソート
                    var options = extractOptions.OrderBy(option => int.Parse(Regex.Replace(option.Replace(root + Path.DirectorySeparatorChar + Shared.Setting.ClassOption.Header, ""), @"[^0-9]", ""))).ToList();

                    // 未選択を追加
                    options.Insert(0, Shared.Select.ClassDefine.UnselectedName);

                    // オプションのディレクトリを走査
                    foreach (var option in options)
                    {
                        // オプション名のディレクトを確認
                        if (Directory.Exists(option) == false)
                        {
                            // オプション名のディレクトリなし ⇒ 未選択を追加
                            _hierarchy.Addition(Shared.Select.ClassDefine.UnselectedName);
                        }
                        else
                        {
                            // オプション名のディレクトリあり ⇒ オプション名を追加
                            _hierarchy.Addition(Path.GetFileName(option));
                        }
                    }
                }
            }
        }
    }
}
