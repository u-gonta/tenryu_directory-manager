using Shared.Tool;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;

namespace Shared
{
    namespace Category
    {
        /// <summary>
        /// 分類のコンボボックス用クラス
        /// </summary>
        public class ClassComboboxItemCategory
        {
            /// <summary>
            /// キー
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// 名称
            /// </summary>
            public string Title { get; set; }

            /// <summary>
            /// ディレクトリのパス
            /// </summary>
            public string Path { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ClassComboboxItemCategory()
            {
                this.Key = Shared.Select.ClassDefine.UnselectedName;
                this.Title = Shared.Select.ClassDefine.UnselectedName;
                this.Path = Shared.Select.ClassDefine.UnselectedName;
            }

            /// <summary>
            /// コンボボックスの項目クラスをコピー
            /// </summary>
            /// <returns></returns>
            public ClassComboboxItemCategory Clone()
            {
                var ret = new ClassComboboxItemCategory();

                ret = (ClassComboboxItemCategory)MemberwiseClone();

                return ret;
            }
        }

        /// <summary>
        /// ディレクトリの分類コントロール
        /// </summary>
        public partial class ControlDirectory : UserControl
        {
            /// <summary>
            /// ディレクトリの分類クラスを管理するクラス
            /// </summary>
            private ClassDirectoryManager _directoryManager = new ClassDirectoryManager();

            /// <summary>
            /// コンボボックスの項目クラス
            /// </summary>
            private ObservableCollection<ClassComboboxItemCategory> _categoryComboboxItems = new ObservableCollection<ClassComboboxItemCategory>();

            /// <summary>
            /// 選択中のディレクトリ名
            /// </summary>
            private string _directorySelected = Shared.Select.ClassDefine.UnselectedName;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ControlDirectory()
            {
                InitializeComponent();

                try
                {
                    // ディレクトリ選択時の通知を登録
                    controlDirectoryTree.DirectorySelected += this.OnSelected;
                }
                catch (Exception ex)
                {
                    // エラーのログを出力
                    Shared.Tool.ClassException.Logging(ex);
                }
            }

            /// <summary>
            /// コンボボックスの選択
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void ComboboxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                try
                {
                    // 選択しているコンボボックスの項目を取得
                    var comboboxItem = comboboxCategory.SelectedItem as ClassComboboxItemCategory;
                    var path = "";

                    // コンボボックスの項目を確認
                    if (comboboxItem != null)
                    {
                        // 選択が有効 ⇒ パスを取得
                        path = comboboxItem.Path;
                    }

                    // ディレクトリを確認
                    if (Directory.Exists(path))
                    {
                        // パスが有効 ⇒ ディレクトリのツリーを更新
                        controlDirectoryTree.Update(new DirectoryInfo(comboboxItem.Path));
                    }
                    else
                    {
                        // パスが無効 ⇒ ディレクトリのツリーを解除
                        controlDirectoryTree.Clear();
                    }
                }
                catch (Exception ex)
                {
                    // エラーのログを出力
                    Shared.Tool.ClassException.Logging(ex);
                }
            }

            /// <summary>
            /// ディレクトリの選択時に呼び出される関数
            /// </summary>
            /// <param name="eventArgs">ディレクトリの選択時に渡されるクラス</param>
            private void OnSelected(Shared.Tool.DirectoryTreeEventArgs args)
            {
                try
                {
                    // ディレクトリを選択
                    controlExplorer.Navigate(args.Path);
                }
                catch (Exception ex)
                {
                    // エラーのログを出力
                    Shared.Tool.ClassException.Logging(ex);
                }
            }

            /// <summary>
            /// 分類名のコンボボックスを更新
            /// </summary>
            /// <param name="directoryManager">ディレクトリの分類クラスを管理するクラス</param>
            public void UpdateCategoryCombobox(ClassDirectoryManager directoryManager)
            {
                // 分類名のクラスをコピー
                _directoryManager = directoryManager.Clone();

                // コンボボックスの項目クラスを初期化
                _categoryComboboxItems = new ObservableCollection<ClassComboboxItemCategory>();

                int index = 0;

                // ディレクトリを走査
                foreach (var directory in _directoryManager.Directories)
                {
                    var comboboxItem = new ClassComboboxItemCategory();

                    comboboxItem.Path = directory.Path;
                    comboboxItem.Title = directory.Title;

                    // ディレクトリのパスを確認
                    if (_directorySelected == directory.Path)
                    {
                        // 選択中のパスと一致
                        index = _categoryComboboxItems.Count;
                    }

                    // コンボボックスの項目を追加
                    _categoryComboboxItems.Add(comboboxItem);
                }

                // コンボボックスの項目を更新
                comboboxCategory.ItemsSource = _categoryComboboxItems;

                // コンボボックスを選択
                comboboxCategory.SelectedIndex = index;
            }
        }
    }
}
