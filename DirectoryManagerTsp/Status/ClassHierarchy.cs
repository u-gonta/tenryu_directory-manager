using System.Collections.Generic;

namespace DirectoryManagerTsp
{
    namespace Status
    {
        /// <summary>
        /// ディレクトリの階層を保持するクラス
        /// </summary>
        public class ClassHierarchy
        {
            /// <summary>
            /// ディレクトリの階層(機種名\装置名\工番名)
            /// </summary>
            private Dictionary<string, Dictionary<string, List<string>>> _names;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassHierarchy()
            {
                // 初期化
                Initialize();
            }

            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // ディレクトリの階層を確認
                if (this._names == null)
                {
                    this._names = new Dictionary<string, Dictionary<string, List<string>>>();
                }
            }

            /// <summary>
            /// ディレクトリの階層を保持するクラスをコピー
            /// </summary>
            /// <returns></returns>
            public ClassHierarchy Clone()
            {
                var ret = new ClassHierarchy();

                ret = (ClassHierarchy)MemberwiseClone();

                // ディレクトリの階層をコピー
                ret._names = new Dictionary<string, Dictionary<string, List<string>>>(this._names);

                return ret;
            }

            /// <summary>
            /// ディレクトリの階層をクリア
            /// </summary>
            public void Clear()
            {
                // ディレクトリの階層をクリア
                this._names.Clear();
            }

            /// <summary>
            /// ディレクトリの階層を追加
            /// </summary>
            /// <param name="model">機種名</param>
            /// <param name="equipment">装置名</param>
            public void AdditionEquipment(string model, string equipment)
            {
                // 機種名を確認
                if (this._names.ContainsKey(model) == false)
                {
                    // 機種名の登録なし ⇒ 機種名を登録
                    this._names[model] = new Dictionary<string, List<string>>();
                }

                // 装置名を確認
                if (this._names[model].ContainsKey(equipment) == false)
                {
                    // 装置名の登録なし ⇒ 装置名を登録
                    this._names[model][equipment] = new List<string>();
                }
            }

            /// <summary>
            /// ディレクトリの階層を追加
            /// </summary>
            /// <param name="model">機種名</param>
            /// <param name="equipment">装置名</param>
            /// <param name="process">工番名</param>
            public void AdditionProcess(string model, string equipment, string process)
            {
                // 機種名を確認
                if (this._names.ContainsKey(model) == false)
                {
                    // 機種名の登録なし ⇒ 機種名を登録
                    this._names[model] = new Dictionary<string, List<string>>();
                }

                // 装置名を確認
                if (this._names[model].ContainsKey(equipment) == false)
                {
                    // 装置名の登録なし ⇒ 装置名を登録
                    this._names[model][equipment] = new List<string>();
                }

                // 工番名を確認
                if (this._names[model][equipment].Exists(name => name == process) == false)
                {
                    // 工番名の登録なし ⇒ 工番名を登録
                    this._names[model][equipment].Add(process);
                    // 工番名をソート
                    this._names[model][equipment].Sort((a, b) => a.CompareTo(b));
                }
            }

            /// <summary>
            /// 機種名を取得
            /// </summary>
            /// <returns>機種名の一覧</returns>
            public List<string> GetModelNames()
            {
                var ret = new List<string>();

                // 装置名をコピー
                ret = new List<string>(_names.Keys);

                return ret;
            }

            /// <summary>
            /// 装置名を取得
            /// </summary>
            /// <param name="model">機種名</param>
            /// <returns>装置名の一覧</returns>
            public List<string> GetEquipmentNames(string model)
            {
                var ret = new List<string>();

                do
                {
                    // 機種名を確認
                    if (_names.ContainsKey(model) == false)
                    {
                        // 機種名の登録なし ⇒ 処理を抜ける
                        break;
                    }

                    // 装置名をコピー
                    ret = new List<string>(_names[model].Keys);
                } while (false);

                return ret;
            }

            /// <summary>
            /// 工番名を取得
            /// </summary>
            /// <param name="model">機種名</param>
            /// <param name="equipment">装置名</param>
            /// <returns>工番名の一覧</returns>
            public List<string> GetProcessNames(string model, string equipment)
            {
                var ret = new List<string>();

                do
                {
                    // 機種名を確認
                    if (_names.ContainsKey(model) == false)
                    {
                        // 機種名の登録なし ⇒ 処理を抜ける
                        break;
                    }

                    // 装置名を確認
                    if (_names[model].ContainsKey(equipment) == false)
                    {
                        // 装置名の登録なし ⇒ 処理を抜ける
                        break;
                    }

                    // 工番名をコピー
                    ret = new List<string>(_names[model][equipment]);
                } while (false);

                return ret;
            }

            /// <summary>
            /// 機種名を探索
            /// </summary>
            /// <param name="process">工番名</param>
            /// <returns>機種名</returns>
            public string ExtractModelName(string process)
            {
                var ret = "";

                // 機種名を走査
                foreach (var model in _names)
                {
                    // 装置名を走査
                    foreach (var equipment in model.Value)
                    {
                        // 工番名を確認
                        if (equipment.Value.Exists(name => process == name))
                        {
                            // 工番名が登録済み ⇒ 機種名を確定
                            ret = model.Key;
                            break;
                        }
                    }
                }

                return ret;
            }

            /// <summary>
            /// 装置名を探索
            /// </summary>
            /// <param name="process">工番名</param>
            /// <returns>装置名</returns>
            public string ExtractEquipmentName(string process)
            {
                var ret = "";

                // 機種名を走査
                foreach (var model in _names)
                {
                    // 装置名を走査
                    foreach (var equipment in model.Value)
                    {
                        // 工番名を確認
                        if (equipment.Value.Exists(name => process == name))
                        {
                            // 工番名が登録済み ⇒ 装置名を確定
                            ret = equipment.Key;
                            break;
                        }
                    }
                }

                return ret;
            }
        }
    }
}
