using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Shared
{
    namespace Setting
    {
        /// <summary>
        /// ローカルの設定クラスを管理するクラス
        /// ※シングルトンのテンプレート用クラスから継承
        /// </summary>
        class ClassWorkerManager : Singleton.ClassTemplate<ClassWorkerManager>
        {
            /// <summary>
            /// 設定を保持するクラスをロックするためのインスタンス
            /// </summary>
            private static object _asyncSetting = new object();

            /// <summary>
            /// 作業者のクラス
            /// </summary>
            private Dictionary<string, ClassWorker> _workers = new Dictionary<string, ClassWorker>();

            /// <summary>
            /// 設定を保持する作業者のクラスを取得
            /// </summary>
            /// <returns>設定を保持する作業者のクラス</returns>
            public ClassWorker GetWorker(string id)
            {
                ClassWorker ret = null;

                // 排他制御
                lock (_asyncSetting)
                {
                    do
                    {
                        // 作業者を確認
                        if (_workers.ContainsKey(id) == false)
                        {
                            // 作業者の登録なし ⇒ 処理を抜ける
                            break;
                        }

                        // 設定を保持する作業者のクラスをコピー
                        ret = _workers[id].Clone();
                    } while (false);
                }

                return ret;
            }

            /// <summary>
            /// 設定を保持する作業者のクラスを更新
            /// </summary>
            /// <param name="id">作業者のid</param>
            /// <param name="value">設定を保持する作業者のクラス</param>
            public void SetWorker(string id, ClassWorker value)
            {
                // 排他制御
                lock (_asyncSetting)
                {
                    // 設定を保持するクラスをコピー
                    _workers[id] = value.Clone();
                }
            }

            /// <summary>
            /// 設定ファイルを読み込み
            /// </summary>
            /// <param name="path">ファイルのパス</param>
            public void Load(string path)
            {
                // ファイルを開く
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Dictionary<string, ClassWorker>));

                    // 排他制御
                    lock (_asyncSetting)
                    {
                        // デシリアライズして読み込み
                        _workers = (Dictionary<string, ClassWorker>)serializer.ReadObject(stream);
                    }
                }
            }

            /// <summary>
            /// 設定ファイルを書き込み
            /// </summary>
            /// <param name="path">ファイルのパス</param>
            public void Save(string path)
            {
                // ディレクトリを作成
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                // ファイルを開く
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    var serializer = new DataContractJsonSerializer(typeof(Dictionary<string, ClassWorker>));

                    // 排他制御
                    lock (_asyncSetting)
                    {
                        // シリアライズして書き込み
                        serializer.WriteObject(stream, _workers);
                    }
                }
            }
        }
    }
}
