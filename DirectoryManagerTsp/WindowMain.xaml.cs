using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DirectoryManagerTsp
{
    /// <summary>
    /// WindowMain.xaml の相互作用ロジック
    /// </summary>
    public partial class WindowMain : Window
    {
        /// <summary>
        /// 機種名コンボボックスの項目クラス
        /// </summary>
        private ObservableCollection<ComboboxItem.ClassModel> _modelComboboxItems = new ObservableCollection<ComboboxItem.ClassModel>();

        /// <summary>
        /// 装置名コンボボックスの項目クラス
        /// </summary>
        private ObservableCollection<ComboboxItem.ClassEquipment> _equipmentComboboxItems = new ObservableCollection<ComboboxItem.ClassEquipment>();

        /// <summary>
        /// 工番コンボボックスの項目クラス
        /// </summary>
        private ObservableCollection<ComboboxItem.ClassProcess> _processComboboxItems = new ObservableCollection<ComboboxItem.ClassProcess>();

        /// <summary>
        /// 選択中の機種名
        /// </summary>
        private string _modelSelected = Shared.Select.ClassDefine.UnselectedName;

        /// <summary>
        /// 選択中の装置名
        /// </summary>
        private string _equipmentSelected = Shared.Select.ClassDefine.UnselectedName;

        /// <summary>
        /// 選択中の工番名
        /// </summary>
        private string _processSelected = Shared.Select.ClassDefine.UnselectedName;

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

                // 機種と装置の情報を抽出
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
        /// 機種名のコンボボックスを選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboboxModel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                do
                {
                    string selectedName = Shared.Select.ClassDefine.UnselectedName;

                    // 選択しているコンボボックスの項目を取得
                    var comboboxItem = ((ComboBox)sender).SelectedItem as ComboboxItem.ClassModel;

                    // コンボボックスの項目を確認
                    if (comboboxItem == null)
                    {
                        // コンボボックスの項目が未選択 ⇒ 処理を抜ける
                        break;
                    }

                    // コンボボックスを選択中の項目
                    selectedName = comboboxItem.Value;

                    // 選択中の機種名を確認
                    if (_modelSelected != selectedName)
                    {
                        // 機種名が変更 ⇒ 選択中の機種名を更新
                        _modelSelected = selectedName;

                        // 機種名の概要を画面に反映
                        ReflectedModelComment();

                        // 装置名のコンボボックスを更新
                        UpdateEquipmentCombobox();

                        // 工番のコンボボックスを選択
                        SelectProcessCombobox();
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
        /// 機種の作成ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonModelCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 機種を作成する画面
                var window = new Generation.WindowModel();

                // オーナーを更新
                window.Owner = this;

                // 画面を表示
                if (window.ShowDialog() == true)
                {
                    // 機種と装置の情報を抽出
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
        /// 機種の概要を更新ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonModelComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 機種名を確認
                if (_modelSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 機種は未選択
                    throw new Exception("機種が選択されていません。");
                }

                // 機種の概要を更新する画面
                var window = new Generation.Model.WindowComment(_modelSelected);

                // オーナーを更新
                window.Owner = this;

                // 画面を表示
                if (window.ShowDialog() == true)
                {
                    // 機種の概要を画面に反映
                    ReflectedModelComment();
                }
            }
            catch (Exception ex)
            {
                // エラーのメッセージを表示
                Shared.Tool.ClassException.Message(ex);
            }
        }

        /// <summary>
        /// 装置名のコンボボックスを選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboboxEquipment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                do
                {
                    string selectedName = Shared.Select.ClassDefine.UnselectedName;

                    // 選択しているコンボボックスの項目を取得
                    var comboboxItem = ((ComboBox)sender).SelectedItem as ComboboxItem.ClassEquipment;

                    // コンボボックスの項目を確認
                    if (comboboxItem == null)
                    {
                        // コンボボックスの項目が未選択 ⇒ 処理を抜ける
                        break;
                    }

                    // コンボボックスを選択中の項目
                    selectedName = comboboxItem.Value;

                    // 選択中の装置名を確認
                    if (_equipmentSelected != selectedName)
                    {
                        // 装置名が変更 ⇒ 選択中の装置名を更新
                        _equipmentSelected = selectedName;

                        // 装置の概要を画面に反映
                        ReflectedEquipmentComment();

                        // 工番名のコンボボックスを更新
                        UpdateProcessCombobox();

                        // ディレクトリの分類名を更新
                        UpdateDirectoryCategory();

                        // 工番のコンボボックスを選択
                        SelectProcessCombobox();
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
        /// 装置のリビジョンをアップボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEquipmentRevision_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 機種名を確認
                if (_modelSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 機種は未選択
                    throw new Exception("機種が選択されていません。");
                }

                // 装置名を確認
                if (_equipmentSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 装置は未選択
                    throw new Exception("装置が選択されていません。");
                }

                var statusManager = Status.ClassManager.Instance;
                var hierarchy = statusManager.GetHierarchy();
                var equipmentNames = hierarchy.GetEquipmentNames(_modelSelected);
                var equipmentNumber = Tool.ClassEquipment.Number(_equipmentSelected);
                var equipment = new Generation.ClassEquipment();

                // 装置の情報クラスを更新
                equipment.Model = _modelSelected;
                equipment.Name = _equipmentSelected;

                // 装置名を走査
                foreach (var equipmentName in equipmentNames)
                {
                    // 装置名を確認
                    if (equipmentName == Shared.Select.ClassDefine.UnselectedName)
                    {
                        // 未選択 ⇒ 次の装置名へ
                        continue;
                    }

                    // 装置の番号を比較
                    if (equipmentNumber != Tool.ClassEquipment.Number(equipmentName))
                    {
                        // 不一致 ⇒ 次の装置名へ
                        continue;
                    }

                    // 装置名を更新
                    equipment.Name = equipmentName;
                }

                // 装置名を確認
                if (_equipmentSelected != equipment.Name)
                {
                    // リビジョンが更新されている装置名が存在する
                    throw new Exception("既にリビジョンが更新されています。");
                }

                // 装置のリビジョンをアップする画面
                var window = new Generation.Equipment.WindowRevisionUp(equipment.Model, equipment.Name);

                // オーナーを更新
                window.Owner = this;

                // 画面を表示
                if (window.ShowDialog() == true)
                {
                    // 機種と装置の情報を抽出
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
        /// 装置の派生を作成ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEquipmentDerived_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 機種名を確認
                if (_modelSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 機種は未選択
                    throw new Exception("機種が選択されていません。");
                }

                // 装置名を確認
                if (_equipmentSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 装置は未選択
                    throw new Exception("装置が選択されていません。");
                }

                // 装置の派生を作成する画面
                var window = new Generation.Equipment.WindowDerived(_modelSelected, _equipmentSelected);

                // オーナーを更新
                window.Owner = this;

                // 画面を表示
                if (window.ShowDialog() == true)
                {
                    // 機種と装置の情報を抽出
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
        private void buttonEquipmentInformation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 機種名を確認
                if (_modelSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 機種は未選択
                    throw new Exception("機種が選択されていません。");
                }

                // 装置名を確認
                if (_equipmentSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 装置は未選択
                    throw new Exception("装置が選択されていません。");
                }

                var window = new Generation.WindowInformation(_modelSelected, _equipmentSelected);

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
        /// 装置の概要を更新ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonEquipmentComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 機種名を確認
                if (_modelSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 機種は未選択
                    throw new Exception("機種が選択されていません。");
                }

                // 装置名を確認
                if (_equipmentSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 装置は未選択
                    throw new Exception("装置が選択されていません。");
                }

                // 装置の概要を更新する画面
                var window = new Generation.Equipment.WindowComment(_modelSelected, _equipmentSelected);

                // オーナーを更新
                window.Owner = this;

                // 画面を表示
                if (window.ShowDialog() == true)
                {
                    // 装置の概要を画面に反映
                    ReflectedEquipmentComment();
                }
            }
            catch (Exception ex)
            {
                // エラーのメッセージを表示
                Shared.Tool.ClassException.Message(ex);
            }
        }

        /// <summary>
        /// 工番名のコンボボックスを選択
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboboxProcess_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                do
                {
                    string selectedName = Shared.Select.ClassDefine.UnselectedName;

                    // 選択しているコンボボックスの項目を取得
                    var comboboxItem = ((ComboBox)sender).SelectedItem as ComboboxItem.ClassProcess;

                    // コンボボックスの項目を確認
                    if (comboboxItem == null)
                    {
                        // コンボボックスの項目が未選択 ⇒ 処理を抜ける
                        break;
                    }

                    // コンボボックスを選択中の項目
                    selectedName = comboboxItem.Value;

                    // 選択中の工番名を確認
                    if (_processSelected != selectedName)
                    {
                        // 工番名が変更 ⇒ 選択中の工番名を変更
                        _processSelected = selectedName;

                        // 工番名を確認
                        if (_processSelected != Shared.Select.ClassDefine.UnselectedName)
                        {
                            var statusManager = Status.ClassManager.Instance;
                            var hierarchy = statusManager.GetHierarchy();
                            var modelName = hierarchy.ExtractModelName(_processSelected);
                            var equipmentName = hierarchy.ExtractEquipmentName(_processSelected);
                        
                            // 選択中の機種名と選択中の工番が含まれる機種名を確認
                            if (_modelSelected != modelName)
                            {
                                // 機種名が異なる ⇒ 選択中の機種名を更新
                                _modelSelected = modelName;

                                // 選択中の装置名を更新
                                _equipmentSelected = equipmentName;

                                // 機種名のコンボボックスを選択
                                SelectModelCombobox();

                                // 装置名のコンボボックスを更新
                                UpdateEquipmentCombobox();

                                // 処理を抜ける
                                break;
                            }

                            // 選択中の装置名と選択中の工番が含まれる装置名を確認
                            if (_equipmentSelected != equipmentName)
                            {
                                // 装置名が異なる ⇒ 選択中の装置名を更新
                                _equipmentSelected = equipmentName;

                                // 装置名のコンボボックスを選択
                                SelectMachineCombobox();

                                // 処理を抜ける
                                break;
                            }
                        }

                        // 工番の概要を画面に反映
                        ReflectedProcessComment();
                    }
                } while (false);
            }
            catch (Exception ex)
            {
                // エラーのメッセージを表示
                Shared.Tool.ClassException.Message(ex);
            }
        }

        /// <summary>
        /// 工番の作成をアップボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonProcessCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 機種名を確認
                if (_modelSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 機種は未選択
                    throw new Exception("機種が選択されていません。");
                }

                // 装置名を確認
                if (_equipmentSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 装置は未選択
                    throw new Exception("装置が選択されていません。");
                }

                // 工番を作成する画面
                var window = new Generation.Process.WindowCreate(_modelSelected, _equipmentSelected);

                // オーナーを更新
                window.Owner = this;

                // 画面を表示
                if (window.ShowDialog() == true)
                {
                    // 機種と装置の情報を抽出
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
        /// 工番の概要を更新ボタンをクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonProcessComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 機種名を確認
                if (_modelSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 機種は未選択
                    throw new Exception("機種が選択されていません。");
                }

                // 装置名を確認
                if (_equipmentSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 装置は未選択
                    throw new Exception("装置が選択されていません。");
                }

                // 工番名を確認
                if (_processSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 工番は未選択
                    throw new Exception("工番が選択されていません。");
                }

                // 工番の概要を更新する画面
                var window = new Generation.Process.WindowComment(_modelSelected, _equipmentSelected, _processSelected);

                // オーナーを更新
                window.Owner = this;

                // 画面を表示
                if (window.ShowDialog() == true)
                {
                    // 工番の概要を画面に反映
                    ReflectedProcessComment();
                }
            }
            catch (Exception ex)
            {
                // エラーのメッセージを表示
                Shared.Tool.ClassException.Message(ex);
            }
        }

        /// <summary>
        /// 機種と装置の情報を抽出
        /// </summary>
        private void Extract()
        {
            var settingManager = Setting.ClassIntegrationManager.Instance;
            var integration = settingManager.GetIntegration();
            var statusManager = Status.ClassManager.Instance;

            // 機種と装置の情報を抽出
            statusManager.ExtractHierarchy(integration.TemplateRootDirectory, integration.TemplateProcessDirectory);

            // 機種名のコンボボックスを更新
            UpdateModelCombobox();

            // 装置名のコンボボックスを更新
            UpdateEquipmentCombobox();

            // 工番名のコンボボックスを更新
            UpdateProcessCombobox();
        }

        /// <summary>
        /// 作業者を更新
        /// </summary>
        private void UpdateWorker()
        {
            var workerManager = Shared.Setting.ClassWorkerManager.Instance;
            var localManager = Setting.ClassLocalManager.Instance;
            var signIn = false;
            var visibleModelCreate = Visibility.Hidden;
            var visibleModelComment = Visibility.Hidden;
            var visibleEquipmentCreate = Visibility.Hidden;
            var visibleEquipmentComment = Visibility.Hidden;
            var visibleProcessCreate = Visibility.Hidden;
            var visibleProcessComment = Visibility.Hidden;

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
                        case Shared.Setting.ClassAuthority.EnumCategory.CreateModel:
                            // 機種の作成 ⇒ 作成ボタンを有効
                            visibleModelCreate = Visibility.Visible;
                            break;

                        case Shared.Setting.ClassAuthority.EnumCategory.EditModelComment:
                            // 機種のコメント編集 ⇒ コメントの編集ボタンを有効
                            visibleModelComment = Visibility.Visible;
                            break;

                        case Shared.Setting.ClassAuthority.EnumCategory.CreateEquipment:
                            // 装置の作成 ⇒ 作成ボタンを有効
                            visibleEquipmentCreate = Visibility.Visible;
                            break;

                        case Shared.Setting.ClassAuthority.EnumCategory.EditEquipmentComment:
                            // 装置のコメント編集 ⇒ コメントの編集ボタンを有効
                            visibleEquipmentComment = Visibility.Visible;
                            break;

                        case Shared.Setting.ClassAuthority.EnumCategory.CreateProcess:
                            // 工番の作成 ⇒ 作成ボタンを有効
                            visibleProcessCreate = Visibility.Visible;
                            break;

                        case Shared.Setting.ClassAuthority.EnumCategory.EditProcessComment:
                            // 工番のコメント編集 ⇒ コメントの編集ボタンを有効
                            visibleProcessComment = Visibility.Visible;
                            break;
                    }
                }
            } while (false);

            // 機種の作成を確認
            if (visibleModelCreate == Visibility.Visible)
            {
                // 機種の作成が有効 ⇒ コメント＆ボタンを有効
                visibleModelComment = visibleModelCreate;
                visibleEquipmentCreate = visibleModelCreate;
            }

            // 機種のコメントを確認
            if (visibleModelComment == Visibility.Visible)
            {
                // 機種のコメントが有効 ⇒ コメントを有効
                visibleEquipmentComment = visibleModelComment;
            }

            // 装置の作成を確認
            if (visibleEquipmentCreate == Visibility.Visible)
            {
                // 装置の作成が有効 ⇒ コメント＆ボタンを有効
                visibleEquipmentComment = visibleEquipmentCreate;
                visibleProcessCreate = visibleEquipmentCreate;
            }

            // 装置のコメントを確認
            if (visibleEquipmentComment == Visibility.Visible)
            {
                // 装置のコメントが有効 ⇒ コメントを有効
                visibleProcessComment = visibleEquipmentComment;
            }

            // 工番の作成を確認
            if (visibleProcessCreate == Visibility.Visible)
            {
                // 工番の作成が有効 ⇒ コメントを有効
                visibleProcessComment = visibleProcessCreate;
            }

            // コメントの編集コントロールを更新
            buttonModelComment.Visibility = visibleModelComment;
            buttonEquipmentComment.Visibility = visibleEquipmentComment;
            buttonProcessComment.Visibility = visibleProcessComment;

            // 作成のコントロールを更新
            buttonModelCreate.Visibility = visibleModelCreate;
            buttonEquipmentRevision.Visibility = visibleEquipmentCreate;
            buttonEquipmentDerived.Visibility = visibleEquipmentCreate;
            buttonEquipmentInformation.Visibility = visibleEquipmentCreate;
            buttonProcessCreate.Visibility = visibleProcessCreate;

            // 認証のコントロールを更新
            buttonLogin.IsEnabled = !signIn;
            buttonLogout.IsEnabled = signIn;
        }

        /// <summary>
        /// 機種名のコンボボックスを更新
        /// </summary>
        private void UpdateModelCombobox()
        {
            // コンボボックスの項目クラスを初期化
            _modelComboboxItems = new ObservableCollection<ComboboxItem.ClassModel>();

            var statusManager = Status.ClassManager.Instance;
            var hierarchy = statusManager.GetHierarchy();
            int index = 0;

            // 機種名を走査
            foreach (var modelName in hierarchy.GetModelNames())
            {
                var comboboxItem = new ComboboxItem.ClassModel();

                comboboxItem.Value = modelName;
                comboboxItem.Title = modelName;

                // 機種名を確認
                if (_modelSelected == modelName)
                {
                    // 選択中の機種と一致
                    index = _modelComboboxItems.Count;
                }

                // コンボボックスの項目を追加
                _modelComboboxItems.Add(comboboxItem);
            }

            // コンボボックスの項目を更新
            comboboxModel.ItemsSource = _modelComboboxItems;

            // コンボボックスを選択
            comboboxModel.SelectedIndex = index;
        }

        /// <summary>
        /// 機種名の概要を画面に反映
        /// </summary>
        private void ReflectedModelComment()
        {
            var settingManager = Setting.ClassIntegrationManager.Instance;
            var integration = settingManager.GetIntegration();
            var statusManager = Status.ClassManager.Instance;
            var hierarchy = statusManager.GetHierarchy();
            var modelNames = hierarchy.GetModelNames();
            var information = new Generation.ClassDirectory();

            do
            {
                // 機種名の番号を取得
                var index = modelNames.FindIndex(name => name == _modelSelected);

                // 機種名の番号を確認
                if (index < 0)
                {
                    // 機種名の登録がない ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // 機種名を取得
                var modelName = modelNames[index];

                // 機種名を確認
                if (Shared.Select.ClassDefine.UnselectedName == modelName)
                {
                    // 機種が未選択 ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // テンプレートのルートディレクトリを取得
                var directory = integration.TemplateRootDirectory;

                // ディレクトリの区切りを付加
                directory = Shared.Tool.ClassPath.DirectoryDelimiter(directory);

                // 機種名を付加
                directory += modelName;

                // 情報のファイル読み込み
                information = Generation.ClassManager.LoadDirectory(directory);
            } while (false);

            string comment = "";

            // 概要を取得
            comment = information.Comment;
            labelModelComment.Content = comment;
        }

        /// <summary>
        /// 装置の概要を画面に反映
        /// </summary>
        private void ReflectedEquipmentComment()
        {
            var settingManager = Setting.ClassIntegrationManager.Instance;
            var integration = settingManager.GetIntegration();
            var statusManager = Status.ClassManager.Instance;
            var hierarchy = statusManager.GetHierarchy();
            var modelNames = hierarchy.GetModelNames();
            var information = new Generation.ClassDirectory();

            do
            {
                // 機種名の番号を取得
                var index = modelNames.FindIndex(name => name == _modelSelected);

                // 機種名の番号を確認
                if (index < 0)
                {
                    // 機種名の登録がない ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // 機種名を取得
                var modelName = modelNames[index];

                // 機種名を確認
                if (Shared.Select.ClassDefine.UnselectedName == modelName)
                {
                    // 機種が未選択 ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // 機種名の登録あり ⇒ 装置名を取得
                var equipmentNames = hierarchy.GetEquipmentNames(modelName);

                // 機種名の番号を取得
                index = equipmentNames.FindIndex(name => name == _equipmentSelected);

                // 装置名の番号を確認
                if (index < 0)
                {
                    // 装置名の登録がない ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // 装置名を取得
                var equipmentName = equipmentNames[index];

                // 装置名を確認
                if (Shared.Select.ClassDefine.UnselectedName == equipmentName)
                {
                    // 機種が未選択 ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // テンプレートのルートディレクトリを取得
                var directory = integration.TemplateRootDirectory;

                // ディレクトリの区切りを付加
                directory = Shared.Tool.ClassPath.DirectoryDelimiter(directory);

                // 機種名を付加
                directory += modelName;

                // ディレクトリの区切りを付加
                directory = Shared.Tool.ClassPath.DirectoryDelimiter(directory);

                // 装置名を付加
                directory += equipmentName;

                // 情報のファイル読み込み
                information = Generation.ClassManager.LoadDirectory(directory);
            } while (false);

            string comment = "";

            // 派生元の装置名を確認
            if (0 < information.DerivationEquipment.Length)
            {
                // 派生元の装置名がある ⇒ 派生元の装置名を付加
                comment = "派生元：" + information.DerivationEquipment;
                comment += "\n";
            }
            // 概要を取得
            comment += information.Comment;
            labelEquipmentComment.Content = comment;
        }

        /// <summary>
        /// 工番の概要を画面に反映
        /// </summary>
        private void ReflectedProcessComment()
        {
            var settingManager = Setting.ClassIntegrationManager.Instance;
            var integration = settingManager.GetIntegration();
            var statusManager = Status.ClassManager.Instance;
            var hierarchy = statusManager.GetHierarchy();
            var modelNames = hierarchy.GetModelNames();
            var information = new Generation.ClassDirectory();

            do
            {
                // 機種名の番号を取得
                var index = modelNames.FindIndex(name => name == _modelSelected);

                // 機種名の番号を確認
                if (index < 0)
                {
                    // 機種名の登録がない ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // 機種名を取得
                var modelName = modelNames[index];

                // 機種名を確認
                if (Shared.Select.ClassDefine.UnselectedName == modelName)
                {
                    // 機種が未選択 ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // 機種名の登録あり ⇒ 装置名を取得
                var equipmentNames = hierarchy.GetEquipmentNames(modelName);

                // 機種名の番号を取得
                index = equipmentNames.FindIndex(name => name == _equipmentSelected);

                // 装置名の番号を確認
                if (index < 0)
                {
                    // 装置名の登録がない ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // 装置名を取得
                var equipmentName = equipmentNames[index];

                // 装置名を確認
                if (Shared.Select.ClassDefine.UnselectedName == equipmentName)
                {
                    // 機種が未選択 ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // 装置名の登録あり ⇒ 工程名を取得
                var processNames = hierarchy.GetProcessNames(modelName, equipmentName);

                // 装置名の番号を取得
                index = processNames.FindIndex(name => name == _processSelected);

                // 工程名の番号を確認
                if (index < 0)
                {
                    // 工程名の登録がない ⇒ 情報のファイルを読み込む処理を抜ける
                    break;
                }

                // 工程名を取得
                var processName = processNames[index];

                // テンプレートのルートディレクトリを取得
                var directory = integration.TemplateRootDirectory;

                // ディレクトリの区切りを付加
                directory = Shared.Tool.ClassPath.DirectoryDelimiter(directory);

                // 機種名を付加
                directory += modelName;

                // ディレクトリの区切りを付加
                directory = Shared.Tool.ClassPath.DirectoryDelimiter(directory);

                // 装置名を付加
                directory += equipmentName;

                // ディレクトリの区切りを付加
                directory = Shared.Tool.ClassPath.DirectoryDelimiter(directory);

                // 工番のディレクトリ名を付加
                directory += integration.TemplateProcessDirectory;

                // ディレクトリの区切りを付加
                directory = Shared.Tool.ClassPath.DirectoryDelimiter(directory);

                // 工番名を付加
                directory += processName;

                // 情報のファイル読み込み
                information = Generation.ClassManager.LoadDirectory(directory);
            } while (false);

            string comment = "";

            // 概要を取得
            comment += information.Comment;
            labelProcessComment.Content = comment;
        }

        /// <summary>
        /// 装置名のコンボボックスを更新
        /// </summary>
        private void UpdateEquipmentCombobox()
        {
            // コンボボックスの項目クラスを初期化
            _equipmentComboboxItems = new ObservableCollection<ComboboxItem.ClassEquipment>();

            var statusManager = Status.ClassManager.Instance;
            var hierarchy = statusManager.GetHierarchy();
            int index = 0;

            // 装置名を走査
            foreach (var equipmentName in hierarchy.GetEquipmentNames(_modelSelected))
            {
                var comboboxItem = new ComboboxItem.ClassEquipment();

                comboboxItem.Model = _modelSelected;
                comboboxItem.Value = equipmentName;
                comboboxItem.Title = equipmentName;

                // 装置名の名称を確認
                if (_equipmentSelected == equipmentName)
                {
                    // 選択中の名称と一致
                    index = _equipmentComboboxItems.Count;
                }

                // コンボボックスの項目を追加
                _equipmentComboboxItems.Add(comboboxItem);
            }

            // コンボボックスの項目を更新
            comboboxEquipment.ItemsSource = _equipmentComboboxItems;

            // コンボボックスを選択
            comboboxEquipment.SelectedIndex = index;
        }

        /// <summary>
        /// 工番のコンボボックスを更新
        /// </summary>
        private void UpdateProcessCombobox()
        {
            // コンボボックスの項目クラスを初期化
            _processComboboxItems = new ObservableCollection<ComboboxItem.ClassProcess>();

            var statusManager = Status.ClassManager.Instance;
            var hierarchy = statusManager.GetHierarchy();
            int index = 0;

            // 工番名を走査
            foreach (var processName in hierarchy.GetProcessNames(_modelSelected, _equipmentSelected))
            {
                var comboboxItem = new ComboboxItem.ClassProcess();

                comboboxItem.Model = _modelSelected;
                comboboxItem.Equipment = _equipmentSelected;
                comboboxItem.Value = processName;
                comboboxItem.Title = processName;

                // 工番名の名称を確認
                if (_processSelected == processName)
                {
                    // 選択中の名称と一致
                    index = _processComboboxItems.Count;
                }

                // コンボボックスの項目を追加
                _processComboboxItems.Add(comboboxItem);
            }

            // コンボボックスの項目を更新
            comboboxProcess.ItemsSource = _processComboboxItems;

            // コンボボックスを選択
            comboboxProcess.SelectedIndex = index;
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
                // 選択中の機種名を確認
                if (Shared.Select.ClassDefine.UnselectedName == _modelSelected)
                {
                    // 機種が未選択 ⇒ 処理を抜ける
                    break;
                }

                // 機種名を取得
                var model = _modelSelected;

                // 選択中の装置名を確認
                if (Shared.Select.ClassDefine.UnselectedName == _equipmentSelected)
                {
                    // 装置が未選択 ⇒ 処理を抜ける
                    break;
                }

                // 装置名を取得
                var equipment = _equipmentSelected;

                var settingManager = Setting.ClassIntegrationManager.Instance;
                var integration = settingManager.GetIntegration();

                // 部署を走査
                foreach (var department in integration.Departments)
                {
                    // ルートのディレクトリを取得
                    var root = department.RootDirectory;

                    // ディレクトリの区切りを付加
                    root = Shared.Tool.ClassPath.DirectoryDelimiter(root);

                    // 機種名を付加
                    root += model;

                    // ディレクトリの区切りを付加
                    root = Shared.Tool.ClassPath.DirectoryDelimiter(root);

                    // 装置名を付加
                    root += equipment;

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

        /// <summary>
        /// 機種名を選択
        /// ※選択中の工番名に合わせる
        /// </summary>
        private void SelectModelCombobox()
        {
            string selectName = _modelSelected;

            do
            {
                // 選択中の工番を確認
                if (_processSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 工番は未選択 ⇒ 処理を抜ける
                    break;
                }

                //// 選択中の工番名を確認
                //if (_setting.Processies.ContainsKey(_processSelected) == false)
                //{
                //    // 工番の登録なし ⇒ 処理を抜ける
                //    break;
                //}

                //// 選択中の工番を確認
                //if (_setting.Processies[_processSelected].ModelName == _modelSelected)
                //{
                //    // 選択中の機種名と一致 ⇒ 処理を抜ける
                //    break;
                //}

                //// 選択中の工番が含まれる機種名で更新
                //selectName = _setting.Processies[_processSelected].ModelName;
            } while (false);

            string selectedName = Shared.Select.ClassDefine.UnselectedName;

            // 選択しているコンボボックスの項目を取得
            var comboboxItem = comboboxModel.SelectedItem as ComboboxItem.ClassModel;

            // コンボボックスの項目を確認
            if (comboboxItem != null)
            {
                // コンボボックスの項目を選択中
                selectedName = comboboxItem.Value;
            }

            // 選択中の機種名を確認
            if (selectedName != selectName)
            {
                // 選択前と選択後が違う ⇒ 選択中の機種名を更新
                _modelSelected = selectName;

                // 機種名のコンボボックスを走査
                foreach (ComboboxItem.ClassModel item in comboboxModel.Items)
                {
                    // 機種名コンボボックスのアイテムを確認
                    if (_modelSelected == item.Value)
                    {
                        // 機種名が一致
                        comboboxModel.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 装置名を選択
        /// ※選択中の工番名に合わせる
        /// </summary>
        private void SelectMachineCombobox()
        {
            string selectName = _equipmentSelected;

            do
            {
                // 選択中の工番を確認
                if (_processSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 工番は未選択 ⇒ 処理を抜ける
                    break;
                }

                //// 選択中の工番を確認
                //if (_setting.Processies.ContainsKey(_processSelected) == false)
                //{
                //    // 工番の登録なし ⇒ 処理を抜ける
                //    break;
                //}

                //// 選択中の工番を確認
                //if (_setting.Processies[_processSelected].ModelName == _modelSelected)
                //{
                //    // 選択中の機種名と一致 ⇒ 処理を抜ける
                //    break;
                //}

                //// 選択中の工番が含まれる機種名で更新
                //selectName = _setting.Processies[_processSelected].ModelName;
            } while (false);

            string selectedName = Shared.Select.ClassDefine.UnselectedName;

            // 選択しているコンボボックスの項目を取得
            var comboboxItem = comboboxEquipment.SelectedItem as ComboboxItem.ClassEquipment;

            // コンボボックスの項目を確認
            if (comboboxItem != null)
            {
                // コンボボックスの項目を選択中
                selectedName = comboboxItem.Value;
            }

            // 選択中の機種名を確認
            if (selectedName != selectName)
            {
                // 選択前と選択後が違う ⇒ 選択中の装置名を更新
                _equipmentSelected = selectName;

                // 装置名のコンボボックスを走査
                foreach (ComboboxItem.ClassEquipment item in comboboxEquipment.Items)
                {
                    // 装置名コンボボックスのアイテムを確認
                    if (_equipmentSelected == item.Value)
                    {
                        // 装置名が一致
                        comboboxEquipment.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 工番名を選択
        /// ※選択中の機種名と装置名に合わせる
        /// </summary>
        private void SelectProcessCombobox()
        {
            string selectName = _processSelected;

            do
            {
                // 選択中の工番を確認
                if (_processSelected == Shared.Select.ClassDefine.UnselectedName)
                {
                    // 工番は未選択 ⇒ 処理を抜ける
                    break;
                }

                //// 選択中の工番を確認
                //if (_setting.Processies.ContainsKey(_processSelected) == false)
                //{
                //    // 工番の登録なし ⇒ 工番を未選択に更新
                //    selectName = ClassProcess.UnselectedName;

                //    // 処理を抜ける
                //    break;
                //}

                //// 選択中の工番を確認
                //if (_setting.Processies[_processSelected].ModelName != _modelSelected)
                //{
                //    // 選択中の機種名と不一致 ⇒ 工番を未選択に更新
                //    selectName = ClassProcess.UnselectedName;

                //    // 処理を抜ける
                //    break;
                //}

                //// 工番の登録あり ⇒ 選択中の工番を確認
                //if (_setting.Processies[_processSelected].MachineName != _equipmentSelected)
                //{
                //    // 選択中の装置名と不一致 ⇒ 工番を未選択に更新
                //    selectName = ClassProcess.UnselectedName;

                //    // 処理を抜ける
                //    break;
                //}
            } while (false);

            string selectedName = Shared.Select.ClassDefine.UnselectedName;

            // 選択しているコンボボックスの項目を取得
            var comboboxItem = comboboxProcess.SelectedItem as ComboboxItem.ClassProcess;

            // コンボボックスの項目を確認
            if (comboboxItem != null)
            {
                // コンボボックスの項目を選択中
                selectedName = comboboxItem.Value;
            }

            // 選択中の機種名を確認
            if (selectedName != selectName)
            {
                // 選択前と選択後が違う ⇒ 選択中の工番名を更新
                _processSelected = selectName;

                // 工番のコンボボックスを走査
                foreach (ComboboxItem.ClassProcess item in comboboxProcess.Items)
                {
                    // 工番コンボボックスのアイテムを確認
                    if (_processSelected == item.Value)
                    {
                        // 工番名が一致 ⇒ 選択中の工番を更新
                        comboboxProcess.SelectedItem = item;

                        // 工番の走査を抜ける
                        break;
                    }
                }
            }
        }
    }
}
