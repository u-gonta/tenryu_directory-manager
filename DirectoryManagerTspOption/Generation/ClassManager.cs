using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectoryManagerTspOption
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
            static public Shared.Generation.Option.ClassDirectory LoadDirectory(string path)
            {
                // ディレクトリの情報ファイルを読み込み
                return LoadDirectory<Shared.Generation.Option.ClassDirectory>(path);
            }

            /// <summary>
            /// オプションのリビジョンを上げる
            /// </summary>
            /// <param name="source">参照するルートディレクトリ</param>
            /// <param name="destinationOption">派生させるオプションの名前</param>
            static public void RevisionUp(string source, string destinationOption)
            {
                var sources = new List<string>();
                var upperRevision = Shared.Tool.ClassOption.Revision(destinationOption);

                // リビジョンを走査
                for (int revision = 0; revision < upperRevision; revision++)
                {
                    // 参照するリビジョンのディレクトリを確定
                    sources.Add(Shared.Tool.ClassOption.Rename(destinationOption, revision));
                }

                // ルートディレクトリ
                var root = source;

                // ディレクトリを派生
                DirectoryDerived<Shared.Generation.Option.ClassDirectory>(root, root, sources, destinationOption, Shared.Generation.EnumCategory.Invalid);
            }

            /// <summary>
            /// オプションを派生する
            /// </summary>
            /// <param name="source">参照するルートディレクトリ</param>
            /// <param name="sourceOption">参照するオプションの名前</param>
            /// <param name="destinationOption">派生させるオプションの名前</param>
            static public void Derived(string source, string sourceOption, string destinationOption)
            {
                var sources = new List<string>();
                var upperRevision = Shared.Tool.ClassOption.Revision(sourceOption);

                // ディレクトリの情報クラス
                var information = new Shared.Generation.Option.ClassDirectory();

                // ルートディレクトリ
                var sourceRoot = source;
                var destinationRoot = source;

                // リビジョンを走査
                for (int revision = 0; revision <= upperRevision; revision++)
                {
                    // 参照する装置のディレクトリ内のパスを登録
                    sources.Add(Shared.Tool.ClassOption.Rename(sourceOption, revision));

                    // 情報のファイル読み込み
                    var loadInformation = LoadDirectory(sourceRoot + Path.DirectorySeparatorChar + sources.Last());

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
                DirectoryDerived<Shared.Generation.Option.ClassDirectory>(sourceRoot, destinationRoot, sources, destinationOption, Shared.Generation.EnumCategory.Invalid, false);

                // 情報のファイルを書き込み
                SaveDirectory(information, destinationRoot + destinationOption);
            }
        }
    }
}
