using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Shared
{
    namespace Generation
    {
        namespace Manager
        {
            /// <summary>
            /// 生成する情報を管理するテンプレート用クラス
            /// </summary>
            class ClassTemplate
            {
                /// <summary>
                /// ディレクトリの情報ファイル
                /// </summary>
                public const string DirectoryInformationName = "Information.json";

                /// <summary>
                /// ディレクトリの情報ファイルを読み込み
                /// </summary>
                /// <param name="path">ディレクトリのパス</param>
                /// <returns>ディレクトリの情報クラス</returns>
                static public T LoadDirectory<T>(string path) where T : new()
                {
                    var ret = new T();

                    do
                    {
                        // ディレクトリの区切りを付加
                        path = Tool.ClassPath.DirectoryDelimiter(path);

                        // 情報のファイル名を付加
                        path += DirectoryInformationName;

                        // ファイルを確認
                        if (File.Exists(path) == false)
                        {
                            // ファイルなし ⇒ 処理を抜ける
                            break;
                        }

                        // ファイルを開く
                        using (var stream = new FileStream(path, FileMode.Open))
                        {
                            var serializer = new DataContractJsonSerializer(typeof(T));

                            // デシリアライズして読み込み
                            ret = (T)serializer.ReadObject(stream);
                        }
                    } while (false);

                    return ret;
                }

                /// <summary>
                /// ディレクトリの情報ファイルを書き込み
                /// </summary>
                /// <param name="directory">ディレクトリの情報クラス</param>
                /// <param name="path">ディレクトリのパス</param>
                static public void SaveDirectory<T>(T directory, string path)
                {
                    do
                    {
                        // ディレクトリの区切りを付加
                        path = Tool.ClassPath.DirectoryDelimiter(path);

                        // 情報のファイル名を付加
                        path += DirectoryInformationName;

                        // ファイルを確認
                        if (File.Exists(path))
                        {
                            // ファイルあり ⇒ 属性を解除
                            File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.Hidden);
                        }

                        // ファイルを開く
                        using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            var serializer = new DataContractJsonSerializer(typeof(T));

                            // シリアライズして書き込み
                            serializer.WriteObject(stream, directory);
                        }

                        // ファイルに隠し属性を追加
                        File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
                    } while (false);
                }

                /// <summary>
                /// 装置のディレクトリを派生
                /// </summary>
                /// <param name="sourceRoot">参照するルートディレクトリ</param>
                /// <param name="destinationRoot">派生させるルートディレクトリ</param>
                /// <param name="sourceRevisions">参照するリビジョンのディレクトリ名</param>
                /// <param name="destinationRevision">派生させるリビジョン名</param>
                /// <param name="category">生成する種類</param>
                /// <param name="link">true = リンクを有効</param>
                static protected void DirectoryDerived<T>(string sourceRoot, string destinationRoot, List<string> sourceRevisions, string destinationRevision, EnumCategory category, bool link = true) where T : new()
                {
                    var information = new T();

                    // ディレクトリの区切りを付加
                    sourceRoot = Tool.ClassPath.DirectoryDelimiter(sourceRoot);

                    // ディレクトリの区切りを付加
                    destinationRoot = Tool.ClassPath.DirectoryDelimiter(destinationRoot);

                    // リビジョンを走査
                    foreach (var sourceRevision in sourceRevisions)
                    {
                        // 参照するパスを生成
                        var root = sourceRoot + sourceRevision;

                        // 情報のファイル読み込み
                        var loadInformation = LoadDirectory<T>(root);

                        // ディレクトリを走査
                        foreach (var directory in ((Directory.ClassTemplate)(object)loadInformation).Directories)
                        {
                            // ディレクトリの情報を更新
                            ((Directory.ClassTemplate)(object)information).Directories[directory.Key] = directory.Value;
                        }

                        // ファイルを走査
                        foreach (var file in ((Directory.ClassTemplate)(object)loadInformation).Files)
                        {
                            // ファイルの情報を更新
                            ((Directory.ClassTemplate)(object)information).Files[file.Key] = file.Value;
                        }
                    }

                    // リビジョンを走査
                    foreach (var sourceRevision in sourceRevisions)
                    {
                        // 参照するパスを生成
                        var root = sourceRoot + sourceRevision;

                        // 参照するパスを確認
                        if (System.IO.Directory.Exists(root) == false)
                        {
                            // 参照するパスなし ⇒ 次の参照するパスへ
                            continue;
                        }

                        // ディレクトリ内のディレクトリを探索
                        IEnumerable<string> directories = System.IO.Directory.EnumerateDirectories(root, "*");
                        // ディレクトリ内のファイルを探索
                        IEnumerable<string> files = System.IO.Directory.EnumerateFiles(root, "*");

                        // ディレクトリを走査
                        foreach (var directory in directories)
                        {
                            // ディレクトリ名へ分割
                            var name = Path.GetFileName(directory);

                            // ディレクトリを生成する種類を初期化
                            var destinationCategory = EnumCategory.Invalid;

                            // 生成する種類を確認
                            if (EnumCategory.Copy == category)
                            {
                                // コピーを生成 ⇒ 生成する種類を更新
                                destinationCategory = category;
                            }
                            else if (((Directory.ClassTemplate)(object)information).Directories.ContainsKey(name))
                            {
                                // ディレクトリが登録済み ⇒ 生成する種類を更新
                                destinationCategory = ((Directory.ClassTemplate)(object)information).Directories[name].Category;
                            }

                            // 派生するディレクトリの名前
                            string destinationRecursion = Tool.ClassPath.DirectoryDelimiter(destinationRevision) + name;

                            // 生成する種類を確認
                            if (destinationCategory != EnumCategory.Invalid)
                            {
                                // 無効ではない ⇒ ディレクトリを作成
                                System.IO.Directory.CreateDirectory(destinationRoot + destinationRecursion);
                            }

                            // 生成する種類とリンクを確認
                            if (destinationCategory == EnumCategory.Link && link == true)
                            {
                                // リンクを生成 ⇒ リンクを作成
                                var shell = new IWshRuntimeLibrary.WshShell();
                                var shortcut = shell.CreateShortcut(destinationRoot + Tool.ClassPath.DirectoryDelimiter(destinationRecursion) + name + ".lnk");

                                shortcut.TargetPath = Tool.ClassPath.DirectoryDelimiter(root) + name;
                                shortcut.Save();
                            }

                            // 参照するディレクトリの名前
                            var sourceRecursions = new List<string>();

                            // リビジョンを走査
                            foreach (var buffer in sourceRevisions)
                            {
                                // 参照するディレクトリ名を確定
                                sourceRecursions.Add(Tool.ClassPath.DirectoryDelimiter(buffer) + name);
                            }

                            // ディレクトリを派生
                            DirectoryDerived<T>(sourceRoot, destinationRoot, sourceRecursions, destinationRecursion, destinationCategory, link);
                        }

                        // ファイルを走査
                        foreach (var file in files)
                        {
                            // ファイル名へ分割
                            var name = Path.GetFileName(file);

                            // ディレクトリ名と比較
                            if (Path.GetFileName(root) + ".lnk" == name)
                            {
                                // ディレクトリ名と一致 ⇒ 次の処理へ移行
                                continue;
                            }

                            // ディレクトリの情報ファイルと比較
                            if (DirectoryInformationName == name)
                            {
                                // 情報のファイル ⇒ 次の処理へ移行
                                continue;
                            }

                            // ファイルを生成する種類を初期化
                            var destinationCategory = EnumCategory.Invalid;

                            // 生成する種類を確認
                            if (EnumCategory.Copy == category)
                            {
                                // コピーを生成 ⇒ 生成する種類を更新
                                destinationCategory = category;
                            }

                            // ファイルの登録を確認
                            if (((Directory.ClassTemplate)(object)information).Files.ContainsKey(name))
                            {
                                // ファイルが登録済み ⇒ 生成する種類を更新
                                destinationCategory = ((Directory.ClassTemplate)(object)information).Files[name].Category;
                            }

                            // 生成する種類とリンクを確認
                            if (((destinationCategory == EnumCategory.Link) || (category == EnumCategory.Link)) && link == false)
                            {
                                // ファイルもしくは親ディレクトリがリンクを生成＆リンクが無効 ⇒ 生成する種類をコピーへ変更
                                destinationCategory = EnumCategory.Copy;
                            }

                            // 生成する種類を確認
                            if (destinationCategory == EnumCategory.Invalid)
                            {
                                // 無効 ⇒ 次の処理へ移行
                                continue;
                            }

                            // 参照先のパス
                            string sourcePath = Tool.ClassPath.DirectoryDelimiter(root) + name;

                            // 派生先のパス ※ディレクトリまでのパスを生成
                            string destinationPath = destinationRoot + destinationRevision;

                            // ディレクトリを作成
                            System.IO.Directory.CreateDirectory(destinationPath);

                            // 派生先のパスにファイル名を付加
                            destinationPath = Tool.ClassPath.DirectoryDelimiter(destinationPath) + name;

                            // 生成する種類を確認
                            switch (destinationCategory)
                            {
                                case EnumCategory.Copy:
                                    // コピーを生成 ⇒ ファイルを確認
                                    if (File.Exists(destinationPath))
                                    {
                                        // 読み取りの属性を解除
                                        File.SetAttributes(destinationPath, File.GetAttributes(destinationPath) & ~FileAttributes.ReadOnly);
                                    }

                                    // ファイルをコピー
                                    File.Copy(sourcePath, destinationPath, true);
                                    break;

                                case EnumCategory.Link:
                                    // リンクを生成 ⇒ リンクを作成
                                    var shell = new IWshRuntimeLibrary.WshShell();
                                    var shortcut = shell.CreateShortcut(destinationPath + ".lnk");

                                    shortcut.TargetPath = sourcePath;
                                    shortcut.Save();
                                    break;
                            }
                        }
                    }

                    // リンクの生成を確認
                    if (link == false)
                    {
                        do
                        {
                            // ディレクトリの情報を確認
                            if (((Directory.ClassTemplate)(object)information).Directories.Count <= 0 && ((Directory.ClassTemplate)(object)information).Files.Count <= 0)
                            {
                                // ディレクトリの情報がない ⇒ 処理を抜ける
                                break;
                            }

                            // リンクの生成が無効
                            var path = destinationRoot;

                            // ディレクトリの区切りを付加
                            path = Tool.ClassPath.DirectoryDelimiter(path);

                            // 機種のディレクトリを付加
                            path += destinationRevision;

                            // 情報のファイルを書き込み
                            SaveDirectory(information, path);
                        } while (false);
                    }
                }
            }
        }
    }
}
