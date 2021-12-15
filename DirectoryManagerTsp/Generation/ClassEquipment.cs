namespace DirectoryManagerTsp
{
    namespace Generation
    {
        /// <summary>
        /// 装置の情報クラス
        /// </summary>
        class ClassEquipment
        {
            /// <summary>
            /// 機種名
            /// </summary>
            public string Model { get; set; }

            /// <summary>
            /// 装置名
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassEquipment()
            {
                // 初期化
                Initialize();
            }

            /// <summary>
            /// 初期化
            /// </summary>
            public void Initialize()
            {
                // 機種名を確認
                if (Model == null)
                {
                    Model = "";
                }

                // 装置名を確認
                if (Name == null)
                {
                    Name = "";
                }
            }

            /// <summary>
            /// 設定のクラスをコピー
            /// </summary>
            /// <returns></returns>
            public ClassEquipment Clone()
            {
                var ret = new ClassEquipment();

                ret = (ClassEquipment)MemberwiseClone();

                return ret;
            }
        }
    }
}
