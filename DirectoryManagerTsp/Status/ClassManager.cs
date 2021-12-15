using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DirectoryManagerTsp
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
            /// 装置の情報を保持するクラスをロックするためのインスタンス
            /// </summary>
            private static object _asyncHierarchy = new object();

            /// <summary>
            /// 機種と装置の階層を保持するクラス
            /// </summary>
            private ClassHierarchy _hierarchy = new ClassHierarchy();

            /// <summary>
            /// 機種と装置の階層を保持するクラスを取得
            /// </summary>
            /// <returns>機種と装置の階層を保持するクラス</returns>
            public ClassHierarchy GetHierarchy()
            {
                var ret = new ClassHierarchy();

                // 排他制御
                lock (_asyncHierarchy)
                {
                    // 機種と装置の階層を保持するクラスをコピー
                    ret = _hierarchy.Clone();
                }

                return ret;
            }

            /// <summary>
            /// 機種と装置の階層を保持するクラスを更新
            /// </summary>
            /// <param name="value">機種と装置の階層を保持するクラス</param>
            public void SetHierarchy(ClassHierarchy value)
            {
                // 排他制御
                lock (_asyncHierarchy)
                {
                    // 機種と装置の階層を保持するクラスをコピー
                    _hierarchy = value.Clone();
                }
            }

            /// <summary>
            /// 機種と装置の階層を抽出
            /// </summary>
            /// <param name="rootDirectory">ルートのディレクトリ</param>
            /// <param name="processName">工番のディレクトリ名</param>
            public void ExtractHierarchy(string rootDirectory, string processName)
            {
                // 排他制御
                lock (_asyncHierarchy)
                {
                    // クリア
                    _hierarchy.Clear();

                    // 機種名のディレクトリを探索
                    var extractModels = new List<string>(Directory.GetDirectories(rootDirectory, Setting.ClassIntegration.ModelHeader + "*", SearchOption.TopDirectoryOnly));

                    // 機種名をソート
                    var models = extractModels.OrderBy(model => int.Parse(Regex.Replace(model.Replace(Shared.Tool.ClassPath.DirectoryDelimiter(rootDirectory) + Setting.ClassIntegration.ModelHeader, ""), @"[^0-9]", ""))).ToList();

                    // 未選択を追加
                    models.Insert(0, Shared.Select.ClassDefine.UnselectedName);

                    // 機種のディレクトリを走査
                    foreach (var model in models)
                    {
                        // 機種名のディレクトを確認
                        if (Directory.Exists(model) == false)
                        {
                            // 機種名のディレクトリなし ⇒ 未選択を追加
                            _hierarchy.AdditionEquipment(Shared.Select.ClassDefine.UnselectedName, Shared.Select.ClassDefine.UnselectedName);
                            _hierarchy.AdditionProcess(Shared.Select.ClassDefine.UnselectedName, Shared.Select.ClassDefine.UnselectedName, Shared.Select.ClassDefine.UnselectedName);
                        }
                        else
                        {
                            // 機種名のディレクトリあり ⇒ 装置名を探索
                            var equipments = new List<string>(Directory.GetDirectories(model, "TS*", SearchOption.TopDirectoryOnly));

                            // 未選択を追加
                            equipments.Insert(0, Shared.Select.ClassDefine.UnselectedName);

                            // 機種名を抜き出し
                            var modelName = Path.GetFileName(model);

                            // 装置のディレクトリを走査
                            foreach (var equipment in equipments)
                            {
                                // 装置名のディレクトを確認
                                if (Directory.Exists(equipment) == false)
                                {
                                    // 装置名のディレクトリなし ⇒ 未選択を追加
                                    _hierarchy.AdditionEquipment(modelName, Shared.Select.ClassDefine.UnselectedName);
                                    _hierarchy.AdditionProcess(modelName, Shared.Select.ClassDefine.UnselectedName, Shared.Select.ClassDefine.UnselectedName);
                                }
                                else
                                {
                                    // 装置名のディレクトリあり ⇒ 機種名を抜き出し
                                    var equipmentName = Path.GetFileName(equipment);

                                    // 機種名\装置名を追加
                                    _hierarchy.AdditionEquipment(modelName, equipmentName);

                                    var processes = new List<string>();

                                    // 未選択を追加
                                    processes.Add(Shared.Select.ClassDefine.UnselectedName);

                                    do
                                    {
                                        // 工番のディレクトリ名を確認
                                        if (processName.Length <= 0)
                                        {
                                            // 工番のディレクトリ名なし ⇒ 工番名の登録を抜ける
                                            break;
                                        }

                                        // 工番のディレクトリパス
                                        var path = Shared.Tool.ClassPath.DirectoryDelimiter(rootDirectory);
                                        path += Shared.Tool.ClassPath.DirectoryDelimiter(modelName);
                                        path += Shared.Tool.ClassPath.DirectoryDelimiter(equipmentName);
                                        path += Shared.Tool.ClassPath.DirectoryDelimiter(processName);

                                        // 工番のディレクトリを確認
                                        if (Directory.Exists(path) == false)
                                        {
                                            // 工番のディレクトリなし ⇒ 工番名の登録を抜ける
                                            break;
                                        }

                                        // 工番名を探索
                                        processes = new List<string>(Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly));

                                        // 未選択を追加
                                        processes.Insert(0, Shared.Select.ClassDefine.UnselectedName);
                                    } while (false);

                                    // 工番のディレクトリを走査
                                    foreach (var process in processes)
                                    {
                                        // 工番名を登録
                                        _hierarchy.AdditionProcess(modelName, equipmentName, Path.GetFileName(process));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
