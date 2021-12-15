using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DirectoryManagerTspOption
{
    /// <summary>
    /// WindowMain.xaml の相互作用ロジック
    /// </summary>
    public partial class WindowMain : Window
    {
        /// <summary>
        /// オプション名コンボボックスの項目クラス
        /// </summary>
        private ObservableCollection<ComboboxItem.ClassOption> _optionComboboxItems = new ObservableCollection<ComboboxItem.ClassOption>();

        /// <summary>
        /// 選択中のオプション名
        /// </summary>
        private string _optionSelected = Shared.Select.ClassDefine.UnselectedName;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WindowMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ウィンドウのロード
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // 作業者を更新
                UpdateWorker();

                // オプションの情報を抽出
                Extract();
            }
            catch (Exception ex)
            {
                // エラーのメッセージを表示
                Shared.Tool.ClassException.Message(ex);
            }
        }

        /// <summary>
        /// ログインのボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var certification = Shared.Certification.ClassManager.Instance;

                // ユーザーIDを取得
                var result = await certification.AcquireToken().ConfigureAwait(false);

                do
                {
                    // ユーザーIDを確認
                    if (result == null)
                    {
                        // ユーザーIDなし ⇒ 処理を抜ける
                        break;
                    }

                    var localManager = Setting.ClassLocalManager.Instance;

                    // アカウント名を更新
                    localManager.Account = result;

                    // 作業者を更新
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        this.UpdateWorker();
                    }));
                } while (false);
            }
            catch (Exception ex)
            {
                // エラーのメッセージを表示
                Shared.Tool.ClassException.Message(ex);
            }
        }

        /// <summary>
        /// ログアウトのボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonLogout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var localManager = Setting.ClassLocalManager.Instance;

                // アカウント名を解除
                localManager.Account = "";

                // 作業者を更新
                UpdateWorker();
            }
            catch (Exception ex)
            {
                // エラーのメッセージを表示
                Shared.Tool.ClassException.Message(ex);
            }
        }

        /// <summary>
        /// オプション名のコンボボックスを選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboboxOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                do
                {
                    string selectedName = Shared.Select.ClassDefine.UnselectedName;

                    // 選択しているコンボボックスの項目を取得
                    var comboboxItem = ((ComboBox)sender).SelectedItem as ComboboxItem.ClassOption;

                    // コンボボックスの項目を確認
                    if (comboboxItem == null)
                    {
                        // コンボボックスの項目が未選択 ⇒ 処理を抜ける
                        break;
                    }

                    // コンボボックスを選択中の項目
                    selectedName = comboboxItem.Value;

                    // 選択中のオプション名を確認
                    if (_optionSelected != selectedName)
                    {
                        // オプション名が変更 ⇒ 選択中の機種名を更新
                        _optionSelected = selectedName;

                        // オプション名の概要を画面に反映
                        ReflectedOptionComment();

                        // ディレクトリの分類名を更新
                        UpdateDirectoryCategory();
                    }
                } while (false);
            }
            catch (Exception ex)
            {
                // エラーのログを出力
                Shared.Tool.ClassException.Logging(ex);
            }
        }

        /// <summary>
        /// オプションのリビジョンをアップボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOptionRevision_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // オプション名を確認
                if (_optionSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // オプションが未選択
                    throw new Exception("オプションが選択されていません。");
                }

                var statusManager = Status.ClassManager.Instance;
                var hierarchy = statusManager.GetHierarchy();
                var names = hierarchy.GetOptionNames();
                var category = Shared.Tool.ClassOption.Number(_optionSelected);
                var variation = Shared.Tool.ClassOption.Variation(_optionSelected);
                var revision = Shared.Tool.ClassOption.Revision(_optionSelected);

                // オプション名を走査
                foreach (var name in names)
                {
                    // オプション名を確認
                    if (name == Shared.Select.ClassDefine.UnselectedName)
                    {
                        // 未選択 ⇒ 次のオプション名へ
                        continue;
                    }

                    // カテゴリ番号を確認
                    if (category != Shared.Tool.ClassOption.Number(name))
                    {
                        // カテゴリ番号が不一致 ⇒ 次のオプション名へ
                        continue;
                    }

                    // バリエーション番号を確認
                    if (variation != Shared.Tool.ClassOption.Variation(name))
                    {
                        // バリエーション番号が不一致 ⇒ 次のオプション名へ
                        continue;
                    }

                    // リビジョン番号を確認
                    if (revision + 1 == Shared.Tool.ClassOption.Revision(name))
                    {
                        // リビジョンが更新されている
                        throw new Exception("既にリビジョンが更新されています。");
                    }
                }

                // オプションのリビジョンをアップする画面
                var window = new Generation.WindowOptionRevisionUp(_optionSelected);

                // オーナーを更新
                window.Owner = this;

                // 画面を表示
                if (window.ShowDialog() == true)
                {
                    // オプションの情報を抽出
                    Extract();
                }
            }
            catch (Exception ex)
            {
                // エラーのメッセージを表示
                Shared.Tool.ClassException.Message(ex);
            }
        }

        /// <summary>
        /// オプションの派生を作成ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOptionDerived_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // オプション名を確認
                if (_optionSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // オプションが未選択
                    throw new Exception("オプションが選択されていません。");
                }

                // 装置の派生を作成する画面
                var window = new Generation.Option.WindowDerived(_optionSelected);

                // オーナーを更新
                window.Owner = this;

                // 画面を表示
                if (window.ShowDialog() == true)
                {
                    // オプションの情報を抽出
                    Extract();
                }
            }
            catch (Exception ex)
            {
                // エラーのメッセージを表示
                Shared.Tool.ClassException.Message(ex);
            }
        }

        /// <summary>
        /// 対象を設定ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOptionInformation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // オプション名を確認
                if (_optionSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // オプションが未選択
                    throw new Exception("オプションが選択されていません。");
                }

                var window = new Generation.Option.WindowInformation(_optionSelected);

                // オーナーを更新
                window.Owner = this;

                // ウィンドウを表示
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                // エラーのメッセージを表示
                Shared.Tool.ClassException.Message(ex);
            }
        }

        /// <summary>
        /// オプションの概要を更新ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOptionComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // オプション名を確認
                if (_optionSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // オプションが未選択
                    throw new Exception("オプションが選択されていません。");
                }

                // オプションの概要を更新する画面
                var window = new Generation.Option.WindowComment(GetOptionPath(_optionSelected));

                // オーナーを更新
                window.Owner = this;

                // 画面を表示
                if (window.ShowDialog() == true)
                {
                    // オプションの概要を画面に反映
                    ReflectedOptionComment();
                }
            }
            catch (Exception ex)
            {
                // エラーのメッセージを表示
                Shared.Tool.ClassException.Message(ex);
            }
        }

        /// <summary>
        /// オプションの情報を抽出
        /// </summary>
        private void Extract()
        {
            var settingManager = Setting.ClassIntegrationManager.Instance;
            var integration = settingManager.GetIntegration();
            var statusManager = Status.ClassManager.Instance;

            // オプションの情報を抽出
            statusManager.ExtractHierarchy(integration.TemplateDirectory);

            // オプション名のコンボボックスを更新
            UpdateOptionCombobox();
        }

        /// <summary>
        /// 作業者を更新
        /// </summary>
        private void UpdateWorker()
        {
            var workerManager = Shared.Setting.ClassWorkerManager.Instance;
            var localManager = Setting.ClassLocalManager.Instance;
            var signIn = false;
            var visibleOptionCreate = Visibility.Hidden;
            var visibleOptionComment = Visibility.Hidden;

            do
            {
                var account = localManager.Account;
                var worker = workerManager.GetWorker(account);

                // 作業者の登録を確認
                if (worker == null)
                {
                    // 作業差の登録なし ⇒ 処理を抜ける
                    break;
                }

                // サインインを有効
                signIn = true;

                // 権限を走査
                foreach (Shared.Setting.ClassAuthority.EnumCategory category in Enum.GetValues(typeof(Shared.Setting.ClassAuthority.EnumCategory)))
                {
                    // 作業者の権限を確認
                    if (worker.Authoritys.ContainsKey(category) == false)
                    {
                        // 権限なし ⇒ 次の権限へ
                        continue;
                    }

                    // 権限の状態を確認
                    if (worker.Authoritys[category] == false)
                    {
                        // 権限が無効 ⇒ 次の権限へ
                        continue;
                    }

                    // 権限を
                    switch (category)
                    {
                        case Shared.Setting.ClassAuthority.EnumCategory.CreateOption:
                            // オプションの作成 ⇒ 作成ボタンを有効
                            visibleOptionCreate = Visibility.Visible;
                            break;

                        case Shared.Setting.ClassAuthority.EnumCategory.EditOptionComment:
                            // オプションのコメント編集 ⇒ コメントの編集ボタンを有効
                            visibleOptionComment = Visibility.Visible;
                            break;
                    }
                }
            } while (false);

            // オプションの作成を確認
            if (visibleOptionCreate == Visibility.Visible)
            {
                // オプションの作成が有効 ⇒ ボタンを有効
                visibleOptionComment = visibleOptionCreate;
            }

            // コメントの編集コントロールを更新
            buttonOptionComment.Visibility = visibleOptionComment;

            // 作成のコントロールを更新
            buttonOptionRevision.Visibility = visibleOptionCreate;
            buttonOptionDerived.Visibility = visibleOptionCreate;
            buttonOptionInformation.Visibility = visibleOptionCreate;

            // 認証のコントロールを更新
            buttonLogin.IsEnabled = !signIn;
            buttonLogout.IsEnabled = signIn;
        }

        /// <summary>
        /// オプションのディレクトリのパスを取得
        /// </summary>
        /// <param name="option">オプション名</param>
        /// <returns>オプションのディレクトリのパス</returns>
        private string GetOptionPath(string option)
        {
            string ret = "";

            var settingManager = Setting.ClassIntegrationManager.Instance;
            var integration = settingManager.GetIntegration();
            var statusManager = Status.ClassManager.Instance;
            var hierarchy = statusManager.GetHierarchy();
            var options = hierarchy.GetOptionNames();

            do
            {
                // オプション名の番号を取得
                var index = options.FindIndex(name => name == option);

                // オプション名の番号を確認
                if (index < 0)
                {
                    // オプション名の登録がない ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // オプション名を確認
                if (Shared.Select.ClassDefine.UnselectedName == option)
                {
                    // オプションが未選択 ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // テンプレートのディレクトリを取得
                var directory = integration.TemplateDirectory;

                // ディレクトリの区切りを付加
                directory = Shared.Tool.ClassPath.DirectoryDelimiter(directory);

                // オプション名を付加
                ret = directory + option;
            } while (false);

            return ret;
        }

        /// <summary>
        /// オプション名のコンボボックスを更新
        /// </summary>
        private void UpdateOptionCombobox()
        {
            // コンボボックスの項目クラスを初期化
            _optionComboboxItems = new ObservableCollection<ComboboxItem.ClassOption>();

            var statusManager = Status.ClassManager.Instance;
            var hierarchy = statusManager.GetHierarchy();
            int index = 0;

            // オプション名を走査
            foreach (var optionName in hierarchy.GetOptionNames())
            {
                var comboboxItem = new ComboboxItem.ClassOption();

                comboboxItem.Value = optionName;
                comboboxItem.Title = optionName;

                // オプション名を確認
                if (_optionSelected == optionName)
                {
                    // 選択中のオプションと一致
                    index = _optionComboboxItems.Count;
                }

                // コンボボックスの項目を追加
                _optionComboboxItems.Add(comboboxItem);
            }

            // コンボボックスの項目を更新
            comboboxOption.ItemsSource = _optionComboboxItems;

            // コンボボックスを選択
            comboboxOption.SelectedIndex = index;
        }

        /// <summary>
        /// オプション名の概要を画面に反映
        /// </summary>
        private void ReflectedOptionComment()
        {
            var settingManager = Setting.ClassIntegrationManager.Instance;
            var integration = settingManager.GetIntegration();
            var statusManager = Status.ClassManager.Instance;
            var hierarchy = statusManager.GetHierarchy();
            var options = hierarchy.GetOptionNames();
            var information = new Shared.Generation.Option.ClassDirectory();
            var categoryName = "";

            // オプション名を取得
            var option = _optionSelected;

            do
            {
                // オプション名の番号を取得
                var index = options.FindIndex(name => name == option);

                // オプション名の番号を確認
                if (index < 0)
                {
                    // オプション名の登録がない ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // オプション名を確認
                if (Shared.Select.ClassDefine.UnselectedName == option)
                {
                    // オプションが未選択 ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // テンプレートのディレクトリを取得
                var directory = integration.TemplateDirectory;

                // ディレクトリの区切りを付加
                directory = Shared.Tool.ClassPath.DirectoryDelimiter(directory);

                // オプション名を付加
                directory += option;

                // 情報のファイル読み込み
                information = Generation.ClassManager.LoadDirectory(directory);

                // カテゴリ番号を確認
                var categoryNumber = Shared.Tool.ClassOption.Number(option);
                if (integration.Categories.ContainsKey(categoryNumber))
                {
                    // カテゴリ番号の登録あり ⇒ カテゴリ名を取得
                    categoryName = integration.Categories[categoryNumber];
                }
            } while (false);

            var comment = "";

            // カテゴリ名を確認
            if (0 < categoryName.Length)
            {
                // カテゴリ名あり ⇒ 概要を確認
                if (0 < comment.Length)
                {
                    // 概要あり ⇒ 改行を付加
                    comment += "\n";
                }

                // カテゴリ名を概要に付加
                comment += "・" + categoryName;
            }

            // 概要を確認
            if (0 < information.Comment.Length)
            {
                // 概要あり ⇒  概要を確認
                if (0 < comment.Length)
                {
                    // 概要あり ⇒ 改行を付加
                    comment += "\n";
                }

                // 概要を付加
                comment += information.Comment;
            }

            // 概要を画面に反映
            labelOptionComment.Content = comment;
        }

        /// <summary>
        /// ディレクトリの分類名を更新
        /// </summary>
        private void UpdateDirectoryCategory()
        {
            var directory = new Shared.Category.ClassDirectory();
            var directoryManager = new Shared.Category.ClassDirectoryManager();

            // 未選択を追加
            directory.Path = "";
            directory.Title = Shared.Select.ClassDefine.UnselectedName;
            directoryManager.Directories.Add(directory.Clone());

            do
            {
                // 選択中のオプション名を確認
                if (Shared.Select.ClassDefine.UnselectedName == _optionSelected)
                {
                    // オプションが未選択 ⇒ 処理を抜ける
                    break;
                }

                // オプション名を取得
                var option = _optionSelected;

                var settingManager = Setting.ClassIntegrationManager.Instance;
                var integration = settingManager.GetIntegration();

                // 部署を走査
                foreach (var department in integration.Departments)
                {
                    // ルートのディレクトリを取得
                    var root = department.RootDirectory;

                    // ディレクトリの区切りを付加
                    root = Shared.Tool.ClassPath.DirectoryDelimiter(root);

                    // オプション名を付加
                    root += option;

                    // ディレクトリを確認
                    if (Directory.Exists(root) == false)
                    {
                        // ディレクトリなし ⇒ 次の部署へ
                        continue;
                    }

                    // ディレクトリを探索
                    var paths = new List<string>(Directory.GetDirectories(root, "*", SearchOption.TopDirectoryOnly));

                    // ディレクトリを走査
                    foreach (var path in paths)
                    {
                        // パスを更新
                        directory.Path = path;

                        // 名称を更新
                        directory.Title = department.Title;
                        if (0 < directory.Title.Length)
                        {
                            directory.Title += " : ";
                        }
                        directory.Title += Path.GetFileName(path);

                        // ディレクトリのクラスを追加
                        directoryManager.Directories.Add(directory.Clone());
                    }
                }
            } while (false);

            // 分類したディレクトリ名のコンボボックスを更新
            controlDirectoryManager.UpdateCategoryCombobox(directoryManager);
        }
    }
}
