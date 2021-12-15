using System.Windows.Controls;

namespace Shared
{
    namespace Category
    {
        /// <summary>
        /// ControlDirectoryManager.xaml の相互作用ロジック
        /// </summary>
        public partial class ControlDirectoryManager : UserControl
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ControlDirectoryManager()
            {
                InitializeComponent();
            }

            /// <summary>
            /// ディレクトリの分類名コンボボックスを更新
            /// </summary>
            /// <param name="directoryManager">ディレクトリの分類クラスを管理するクラス</param>
            public void UpdateCategoryCombobox(ClassDirectoryManager directoryManager)
            {
                controlDirectory1.UpdateCategoryCombobox(directoryManager);
                controlDirectory2.UpdateCategoryCombobox(directoryManager);
            }
        }
    }
}
