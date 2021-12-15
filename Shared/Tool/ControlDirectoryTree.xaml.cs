using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Shared
{
    namespace Tool
    {
        /// <summary>
        /// ディレクトリの選択時のデリゲートを定義
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        public delegate void DirectoryTreeEventHandler<T>(T args);

        /// <summary>
        /// ディレクトリの選択時に渡されるクラス
        /// </summary>
        public class DirectoryTreeEventArgs
        {
            /// <summary>
            /// ディレクトリのパス
            /// </summary>
            public string Path { get; set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="path">ディレクトリのパス</param>
            public DirectoryTreeEventArgs(string path)
            {
                Path = path;
            }
        }

        /// <summary>
        /// ツリービューのアイテム
        /// </summary>
        public class DirectoryTreeItem : TreeViewItem
        {
            /// <summary>
            /// ディレクトリの選択時に呼び出されるイベント
            /// </summary>
            public event DirectoryTreeEventHandler<DirectoryTreeEventArgs> DirectorySelected;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="directoryInfo">インスタンス</param>
            /// <param name="OnSelected">ディレクトリ選択時に呼び出されるイベント</param>
            public DirectoryTreeItem(DirectoryInfo root, DirectoryTreeEventHandler<DirectoryTreeEventArgs> OnSelected)
            {
                // ディレクトリのパス
                Tag = root.FullName;

                // タイトルを更新
                Header = root.Name;

                // 選択時のイベントを登録
                Selected += Directory_TreeViewItem_Selected;

                // ディレクトリの選択時に呼び出されるイベントを登録
                DirectorySelected += OnSelected;

                // サブディレクトリを取得
                var paths = root.GetDirectories();

                // ディレクトリを走査
                foreach (var path in paths)
                {
                    var attributes = path.Attributes;

                    // 属性を確認
                    if ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        // 隠しフォルダ
                        continue;
                    }

                    // 属性を確認
                    if ((attributes & FileAttributes.System) == FileAttributes.System)
                    {
                        // システムフォルダ
                        continue;
                    }

                    // ディレクトリのアイテムを追加
                    Items.Add(new DirectoryTreeItem(path, OnSelected));
                }
            }

            /// <summary>
            /// ディレクトリの選択時に呼び出される
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Directory_TreeViewItem_Selected(object sender, RoutedEventArgs e)
            {
                do
                {
                    // イベントを確認
                    if (DirectorySelected == null)
                    {
                        // イベントの登録なし
                        break;
                    }

                    var treeViewItem = (DirectoryTreeItem)sender;

                    // 選択を確認
                    if (treeViewItem.IsSelected == false)
                    {
                        // 未選択
                        break;
                    }

                    // イベントを通知
                    DirectorySelected(new DirectoryTreeEventArgs((string)treeViewItem.Tag));
                } while (false);
            }
        }

        /// <summary>
        /// ControlDirectoryTree.xaml の相互作用ロジック
        /// </summary>
        public partial class ControlDirectoryTree : UserControl
        {
            /// <summary>
            /// ディレクトリの選択時に呼び出されるイベント
            /// </summary>
            public event DirectoryTreeEventHandler<DirectoryTreeEventArgs> DirectorySelected;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public ControlDirectoryTree()
            {
                InitializeComponent();
            }

            /// <summary>
            /// ディレクトリをクリア
            /// </summary>
            public void Clear()
            {
                // ツリーをクリア
                TreeView.Items.Clear();

                // ディレクトリの選択を解除
                OnSelected(new DirectoryTreeEventArgs(""));
            }

            /// <summary>
            /// ディレクトリを更新
            /// </summary>
            /// <param name="directoryInfo">インスタンス</param>
            public void Update(DirectoryInfo directoryInfo)
            {
                // ツリーをクリア
                TreeView.Items.Clear();

                // ディレクトリの選択を解除
                OnSelected(new DirectoryTreeEventArgs(""));

                // ツリーにディレクトリを追加
                TreeView.Items.Add(new DirectoryTreeItem(directoryInfo, this.OnSelected));
            }

            /// <summary>
            /// ディレクトリの選択時に呼び出される関数
            /// </summary>
            /// <param name="eventArgs">ディレクトリの選択時に渡されるクラス</param>
            private void OnSelected(DirectoryTreeEventArgs args)
            {
                do
                {
                    // イベントを確認
                    if (DirectorySelected == null)
                    {
                        // イベントの登録なし
                        break;
                    }

                    // イベントを通知
                    DirectorySelected(args);
                } while (false);
            }
        }
    }
}
