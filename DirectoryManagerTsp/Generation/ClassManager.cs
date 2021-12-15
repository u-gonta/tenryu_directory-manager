using System;
using System.Collections.Generic;
using System.Linq;

namespace DirectoryManagerTsp
{
    namespace Generation
    {
        /// <summary>
        /// 生成する情報を管理するクラス
        /// ※生成する情報を管理するテンプレート用クラスから継承
        /// </summary>
        class ClassManager : Shared.Generation.Manager.ClassTemplate
        {
            /// <summary>
            /// ディレクトリの情報ファイルを読み込み
            /// </summary>
            /// <param name="path">ディレクトリのパス</param>
            /// <returns>ディレクトリの情報クラス</returns>
            static public ClassDirectory LoadDirectory(string path)
            {
                // ディレクトリの情報ファイルを読み込み
                return LoadDirectory<ClassDirectory>(path);
            }

            /// <summary>
            /// オプションのリビジョンを上げる
            /// </summary>
            /// <param name="source">参照するルートディレクトリ</param>
            /// <param name="destinationEquipment">派生させる装置の情報クラス</param>
            static public void RevisionUp(string source, ClassEquipment destinationEquipment)
            {
                var sources = new List<string>();
                var upperRevision = Tool.ClassEquipment.Revision(destinationEquipment.Name);

                // リビジョンを走査
                for (int revision = 0; revision < upperRevision; revision++)
                {
                    // 参照するリビジョンのディレクトリを確定
                    sources.Add(Tool.ClassEquipment.Rename(destinationEquipment.Name, revision));
                }

                // ルートディレクトリ
                var root = source;

                // ディレクトリの区切りを付加
                root = Shared.Tool.ClassPath.DirectoryDelimiter(root);

                // 機種名を付加
                root += destinationEquipment.Model;

                // ディレクトリを派生
                DirectoryDerived<ClassDirectory>(root, root, sources, destinationEquipment.Name, Shared.Generation.EnumCategory.Invalid);
            }

            /// <summary>
            /// 装置を派生する
            /// </summary>
            /// <param name="source">参照するルートディレクトリ</param>
            /// <param name="sourceEquipment">参照する装置の情報クラス</param>
            /// <param name="destinationEquipment">派生させる装置の情報クラス</param>
            static public void Derived(string source, ClassEquipment sourceEquipment, ClassEquipment destinationEquipment)
            {
                var sources = new List<string>();
                var upperRevision = Tool.ClassEquipment.Revision(sourceEquipment.Name);

                // ディレクトリの情報クラス
                var information = new ClassDirectory();

                // ルートディレクトリ
                var sourceRoot = source;
                var destinationRoot = source;

                // ディレクトリの区切りを付加
                sourceRoot = Shared.Tool.ClassPath.DirectoryDelimiter(sourceRoot);

                // ディレクトリの区切りを付加
                destinationRoot = Shared.Tool.ClassPath.DirectoryDelimiter(destinationRoot);

                // 機種名を付加
                sourceRoot += sourceEquipment.Model;
                destinationRoot += destinationEquipment.Model;

                // リビジョンを走査
                for (int revision = 0; revision <= upperRevision; revision++)
                {
                    // 参照する装置のディレクトリ内のパスを登録
                    sources.Add(Tool.ClassEquipment.Rename(sourceEquipment.Name, revision));

                    // 情報のファイル読み込み
                    var loadInformation = LoadDirectory(Shared.Tool.ClassPath.DirectoryDelimiter(sourceRoot) + sources.Last());

                    // ディレクトリの情報を統合
                    foreach (var directory in loadInformation.Directories)
                    {
                        // ディレクトリの情報を更新
                        information.Directories[directory.Key] = directory.Value.Clone();
                    }

                    // ファイルの情報を統合
                    foreach (var file in loadInformation.Files)
                    {
                        // ファイルの情報を更新
                        information.Files[file.Key] = file.Value.Clone();
                    }
                }

                // ディレクトリを派生
                DirectoryDerived<ClassDirectory>(sourceRoot, destinationRoot, sources, destinationEquipment.Name, Shared.Generation.EnumCategory.Invalid, false);

                // パスの区切りを付加
                sourceRoot = Shared.Tool.ClassPath.DirectoryDelimiter(sourceRoot);
                destinationRoot = Shared.Tool.ClassPath.DirectoryDelimiter(destinationRoot);

                // 派生元の機種名を付加
                information.DerivationModel = sourceEquipment.Model;

                // 派生元の装置名を付加
                information.DerivationEquipment = sourceEquipment.Name;

                // 情報のファイルを書き込み
                SaveDirectory(information, destinationRoot + destinationEquipment.Name);
            }

            /// <summary>
            /// 装置の番号を抽出
            /// </summary>
            /// <param name="equipments">装置名のリスト</param>
            /// <param name="header">該当する装置名のヘッダ</param>
            /// <returns>装置の番号</returns>
            static public List<int> ExtractEquipmentNumber(List<string> equipments, string header)
            {
                var ret = new List<int>();

                // 装置名を走査
                foreach (var equipment in equipments)
                {
                    // 装置名を確認
                    if (equipment == Shared.Select.ClassDefine.UnselectedName)
                    {
                        // 装置名の未選択 ⇒ 次の装置名へ
                        continue;
                    }

                    // 装置名を分解 ⇒ 装置名のヘッダを除去
                    var name = equipment.Replace(header, "");

                    // 装置名のリビジョン番号を除去
                    var number = int.Parse(name.Remove(name.Length - 1));

                    // 抽出した装置の番号を確認
                    if (0 <= ret.FindIndex(target => number == target))
                    {
                        // 装置の番号が登録済み ⇒ 次の装置名へ
                        continue;
                    }

                    // 装置の番号を追加
                    ret.Add(number);
                }

                return ret;
            }

            /// <summary>
            /// 最新な装置の番号を抽出
            /// </summary>
            /// <param name="equipments">装置名のリスト</param>
            /// <param name="header">該当する装置名のヘッダ</param>
            /// <returns>装置の番号</returns>
            static public int ExtractLastEquipmentNumber(List<string> equipments, string header)
            {
                int ret = -1;

                do
                {
                    // 装置名を確認
                    if (equipments.Count <= 0)
                    {
                        // 装置名なし
                        throw new Exception("装置番号の登録なし");
                    }

                    // 装置の番号を抽出
                    var numbers = ExtractEquipmentNumber(equipments, header);

                    // 装置の番号を確認
                    if (numbers.Count <= 0)
                    {
                        // 該当する装置名のヘッダなし
                        throw new Exception("該当する装置番号の登録なし");
                    }

                    // 最新な装置の番号を取得
                    ret = numbers.Last();
                } while (false);

                return ret;
            }
        }
    }
}
