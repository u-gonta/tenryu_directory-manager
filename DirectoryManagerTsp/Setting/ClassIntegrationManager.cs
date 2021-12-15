using System.IO;
using System.Runtime.Serialization.Json;

namespace DirectoryManagerTsp
{
    namespace Setting
    {
        /// <summary>
        /// 設定を保持するクラスを管理するクラス
        /// ※シングルトンのテンプレート用クラスから継承
        /// </summary>
        public class ClassIntegrationManager : Shared.Singleton.ClassTemplate<ClassIntegrationManager>
        {
            /// <summary>
            /// 設定のファイル
            /// </summary>
            private const string FileName = "Setting.json";

            /// <summary>
            /// 設定を保持するクラスをロックするためのインスタンス
            /// </summary>
            private static object _asyncSetting = new object();

            /// <summary>
            /// 設定を保持するクラス
            /// </summary>
            private ClassIntegration _integration = new ClassIntegration();

            /// <summary>
            /// 設定を保持するクラスを取得
            /// </summary>
            /// <returns>設定を保持するクラス</returns>
            public ClassIntegration GetIntegration()
            {
                var ret = new ClassIntegration();

                // 排他制御
                lock (_asyncSetting)
                {
                    // 設定を保持するクラスをコピー
                    ret = _integration.Clone();
                }

                return ret;
            }

            /// <summary>
            /// 設定を保持するクラスを更新
            /// </summary>
            /// <param name="value">設定を保持するクラス</param>
            public void SetIntegration(ClassIntegration value)
            {
                // 排他制御
                lock (_asyncSetting)
                {
                    // 設定を保持するクラスをコピー
                    _integration = value.Clone();
                }
            }

            /// <summary>
            /// 設定ファイルを読み込み
            /// </summary>
            /// <param name="path">ディレクトリのパス</param>
            public void LoadIntegration(string path)
            {
                // ディレクトリの区切りを付加
                path = Shared.Tool.ClassPath.DirectoryDelimiter(path);

                // 設定のファイル名を付加
                path += FileName;

                // ファイルを開く
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    var serializer = new DataContractJsonSerializer(typeof(ClassIntegration));

                    // 排他制御
                    lock (_asyncSetting)
                    {
                        // デシリアライズして読み込み
                        _integration = (ClassIntegration)serializer.ReadObject(stream);
                    }
                }
            }

            /// <summary>
            /// 設定ファイルを書き込み
            /// </summary>
            /// <param name="path">ディレクトリのパス</param>
            public void SaveIntegration(string path)
            {
                // ディレクトリの区切りを付加
                path = Shared.Tool.ClassPath.DirectoryDelimiter(path);

                // ディレクトリを作成
                Directory.CreateDirectory(path);

                // 設定のファイル名を付加
                path += FileName;

                // ファイルを開く
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    var serializer = new DataContractJsonSerializer(typeof(ClassIntegration));

                    // 排他制御
                    lock (_asyncSetting)
                    {
                        // シリアライズして書き込み
                        serializer.WriteObject(stream, _integration);
                    }
                }
            }
        }
    }
}
