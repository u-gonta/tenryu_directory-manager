using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace DirectoryManagerTsp
{
    namespace Generation
    {
        namespace Equipment
        {
            /// <summary>
            /// WindowDerived.xaml の相互作用ロジック
            /// </summary>
            public partial class WindowDerived : Window
            {
                /// <summary>
                /// 参照する機種名
                /// </summary>
                private string _modelSource = Shared.Select.ClassDefine.UnselectedName;

                /// <summary>
                /// 参照する装置名
                /// </summary>
                private string _equipmentSource = Shared.Select.ClassDefine.UnselectedName;

                /// <summary>
                /// 機種名コンボボックスの項目クラス
                /// </summary>
                private ObservableCollection<ComboboxItem.ClassModel> _modelComboboxItems = new ObservableCollection<ComboboxItem.ClassModel>();

                /// <summary>
                /// 選択中の機種名
                /// </summary>
                private string _modelSelected = Shared.Select.ClassDefine.UnselectedName;

                /// <summary>
                /// 装置の番号コンボボックスの項目クラス
                /// </summary>
                private ObservableCollection<ComboboxItem.Equipment.ClassNumber> _equipmentNumberComboboxItems = new ObservableCollection<ComboboxItem.Equipment.ClassNumber>();

                /// <summary>
                /// コンストラクタ
                /// </summary>
                /// <param name="model">機種名</param>
                /// <param name="equipment">装置名</param>
                public WindowDerived(string model, string equipment)
                {
                    InitializeComponent();

                    try
                    {
                        // 参照する機種名を更新
                        _modelSource = model;

                        // 参照する装置名を更新
                        _equipmentSource = equipment;

                        // 選択中の機種名を更新
                        _modelSelected = model;

                        // 画面を初期化
                        InitializeWindow();

                        // 機種名のコンボボックスを更新
                        UpdateModelCombobox();

                        // 装置の番号を更新
                        UpdateEquipmentNumber();

                        // 派生後の装置の概要を画面に更新
                        ReflectedEquipmentComment();
                    }
                    catch (Exception ex)
                    {
                        // エラーのメッセージを表示
                        Shared.Tool.ClassException.Message(ex);
                    }
                }

                /// <summary>
                /// 機種を作成
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private void buttonModelCreate_Click(object sender, RoutedEventArgs e)
                {
                    try
                    {
                        // 機種を作成する画面
                        var window = new WindowModel();

                        // オーナーを更新
                        window.Owner = this;

                        // 画面を表示
                        if (window.ShowDialog() == true)
                        {
                            var settingManager = Setting.ClassIntegrationManager.Instance;
                            var integration = settingManager.GetIntegration();
                            var statusManager = Status.ClassManager.Instance;

                            // 機種と装置の情報を抽出
                            statusManager.ExtractHierarchy(integration.TemplateRootDirectory, integration.TemplateProcessDirectory);

                            // 機種名のコンボボックスを更新
                            UpdateModelCombobox();

                            // 装置の番号を更新
                            UpdateEquipmentNumber();
                        }
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
                                // コンボボックスの項目が未選択
                                break;
                            }

                            // コンボボックスを選択中の項目
                            selectedName = comboboxItem.Value;

                            // 選択中の機種名を確認
                            if (_modelSelected != selectedName)
                            {
                                // 機種名が変更 ⇒ 選択中の機種名を更新
                                _modelSelected = selectedName;

                                // 派生後の装置の概要を画面に更新
                                ReflectedEquipmentComment();

                                // 装置の番号を更新
                                UpdateEquipmentNumber();
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
                /// 作成ボタンクリック
                /// </summary>
                /// <param name="sender"></param>
                /// <param name="e"></param>
                private void buttonGeneration_Click(object sender, RoutedEventArgs e)
                {
                    try
                    {
                        // 選択しているコンボボックスの項目を取得
                        var comboboxItemModel = comboboxModel.SelectedItem as ComboboxItem.ClassModel;

                        // コンボボックスの項目を確認
                        if (comboboxItemModel == null)
                        {
                            // コンボボックスの項目が未選択
                            throw new Exception("機種名が選択されていません。");
                        }

                        // 選択しているコンボボックスの項目を取得
                        var comboboxItemEquipmentNumber = comboboxEquipmentNumber.SelectedItem as ComboboxItem.Equipment.ClassNumber;

                        // コンボボックスの項目を確認
                        if (comboboxItemEquipmentNumber == null)
                        {
                            // コンボボックスの項目が未選択
                            throw new Exception("装置番号が選択されていません。");
                        }

                        var settingManager = Setting.ClassIntegrationManager.Instance;
                        var integration = settingManager.GetIntegration();

                        // 参照する装置の情報クラス
                        var equipmentSource = new ClassEquipment();

                        equipmentSource.Model = _modelSource;
                        equipmentSource.Name = _equipmentSource;

                        // 参照する装置の情報クラス
                        var equipmentDestination = new ClassEquipment();

                        equipmentDestination.Model = comboboxItemModel.Value;
                        equipmentDestination.Name = labelEquipmentHeaderDestination.Content + comboboxItemEquipmentNumber.Title + labelEquipmentRevisionDestination.Content;

                        // 部署を走査
                        foreach (var department in integration.Departments)
                        {
                            // 部署のディレクトリ内に機種と装置のディレクトリを作成
                            Directory.CreateDirectory(Shared.Tool.ClassPath.DirectoryDelimiter(department.RootDirectory) + Shared.Tool.ClassPath.DirectoryDelimiter(equipmentDestination.Model) + equipmentDestination.Name);

                            // ディレクトリを派生する
                            ClassManager.Derived(department.RootDirectory, equipmentSource, equipmentDestination);
                        }

                        // 派生後の機種ディレクトリを取得
                        var path = GetModelDestinationPath();

                        // ディレクトリを作成
                        Directory.CreateDirectory(path);

                        // 派生後の装置ディレクトリを取得
                        path = GetDestinationPath();

                        // 派生後の装置ディレクトリにある情報のファイル読み込み
                        var information = ClassManager.LoadDirectory(path);

                        // 派生元の機種名を付加
                        information.DerivationModel = _modelSource;

                        // 派生元の装置名を付加
                        information.DerivationEquipment = _equipmentSource;

                        // コメントを付加
                        information.Comment = textBoxComment.Text;

                        // 情報のファイルを書き込み
                        ClassManager.SaveDirectory(information, path);

                        // 戻り値を更新
                        this.DialogResult = true;

                        // 画面を閉じる
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        // エラーのメッセージを表示
                        Shared.Tool.ClassException.Message(ex);
                    }
                }

                /// <summary>
                /// 画面を初期化
                /// </summary>
                private void InitializeWindow()
                {
                    // 参照する機種名を更新
                    labelModelSource.Content = _modelSource;

                    // 参照する装置名を更新
                    labelEquipmentSource.Content = _equipmentSource;

                    // 参照する機種ディレクトリを取得
                    var path = GetModelSourcePath();

                    // 参照する機種ディレクトリにある情報のファイル読み込み
                    var information = ClassManager.LoadDirectory(path);

                    // 参照する機種のコメントを更新
                    labelModelSourceComment.Content = information.Comment;

                    // 参照する装置ディレクトリを取得
                    path = GetSourcePath();

                    // 参照する装置ディレクトリにある情報のファイル読み込み
                    information = ClassManager.LoadDirectory(path);

                    // 派生元の装置名を確認
                    if (0 < information.DerivationEquipment.Length)
                    {
                        // 派生元の装置名がある ⇒ 派生元の装置名を付加
                        labelEquipmentSourceComment.Content = "派生元：" + information.DerivationEquipment;
                        labelEquipmentSourceComment.Content += "\n";
                    }

                    // コメントを更新
                    labelEquipmentSourceComment.Content += information.Comment;
                    textBoxComment.Text = information.Comment;
                }

                /// <summary>
                /// 派生後の装置の概要を画面に反映
                /// </summary>
                private void ReflectedEquipmentComment()
                {
                    // 派生後の機種ディレクトリを取得
                    var path = GetModelDestinationPath();

                    // 派生後の機種ディレクトリにある情報のファイル読み込み
                    var information = ClassManager.LoadDirectory(path);

                    // 派生後の機種コメントを更新
                    labelModelDestinationComment.Content = information.Comment;
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
                        // 機種名を確認
                        if (modelName == Shared.Select.ClassDefine.UnselectedName)
                        {
                            // 機種名が未選択 ⇒ 次の機種へ
                            continue;
                        }

                        ComboboxItem.ClassModel comboboxItem = new ComboboxItem.ClassModel();

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
                /// 装置の番号コンボボックスを更新
                /// </summary>
                private void UpdateEquipmentNumberCombobox()
                {
                    int selected = -1;

                    // コンボボックスの項目クラスを初期化
                    _equipmentNumberComboboxItems = new ObservableCollection<ComboboxItem.Equipment.ClassNumber>();

                    var statusManager = Status.ClassManager.Instance;
                    var hierarchy = statusManager.GetHierarchy();
                    var equipmentNumbers = ClassManager.ExtractEquipmentNumber(hierarchy.GetEquipmentNames(_modelSelected), (string)labelEquipmentHeaderDestination.Content);
                    int index = 0;

                    // 装置の番号を走査
                    for (var equipmentNumber = 0; equipmentNumber <= 99; equipmentNumber++)
                    {
                        // 装置の番号を確認
                        if (0 <= equipmentNumbers.FindIndex(target => target == equipmentNumber))
                        {
                            // 装置の番号が登録済み ⇒ 次の装置の番号へ
                            continue;
                        }

                        var comboboxItem = new ComboboxItem.Equipment.ClassNumber();

                        comboboxItem.Value = equipmentNumber;
                        comboboxItem.Title = equipmentNumber.ToString("D2");

                        // 装置の番号を確認
                        if (selected < 0)
                        {
                            // 選択中の装置の番号が未初期化 ⇒ 初期化
                            selected = equipmentNumber;
                        }
                        if (selected == equipmentNumber)
                        {
                            // 選択中の装置の番号と一致
                            index = _equipmentNumberComboboxItems.Count;
                        }

                        // コンボボックスの項目を追加
                        _equipmentNumberComboboxItems.Add(comboboxItem);
                    }

                    // コンボボックスの項目を更新
                    comboboxEquipmentNumber.ItemsSource = _equipmentNumberComboboxItems;

                    // コンボボックスを選択
                    comboboxEquipmentNumber.SelectedIndex = index;
                }

                /// <summary>
                /// 装置の番号を更新
                /// </summary>
                private void UpdateEquipmentNumber()
                {
                    // 装置名のヘッダを更新
                    labelEquipmentHeaderDestination.Content = Tool.ClassEquipment.Header(_modelSelected);

                    // 装置の番号コンボボックスを更新
                    UpdateEquipmentNumberCombobox();
                }

                /// <summary>
                /// 参照する機種ディレクトリを取得
                /// </summary>
                /// <returns>参照する機種ディレクトリ</returns>
                private string GetModelSourcePath()
                {
                    string ret = "";

                    var settingManager = Setting.ClassIntegrationManager.Instance;
                    var integration = settingManager.GetIntegration();

                    // テンプレートのルートディレクトリ
                    ret = integration.TemplateRootDirectory;

                    // ディレクトリの区切りを付加
                    ret = Shared.Tool.ClassPath.DirectoryDelimiter(ret);

                    // 機種名を付加
                    ret += _modelSource;

                    return ret;
                }

                /// <summary>
                /// 派生後の機種ディレクトリを取得
                /// </summary>
                /// <returns>派生後の機種ディレクトリ</returns>
                private string GetModelDestinationPath()
                {
                    string ret = "";

                    do
                    {
                        // 選択しているコンボボックスの項目を取得
                        var comboboxItemModel = comboboxModel.SelectedItem as ComboboxItem.ClassModel;

                        // コンボボックスの項目を確認
                        if (comboboxItemModel == null)
                        {
                            // コンボボックスの項目が未選択 ⇒ 処理を抜ける
                            break;
                        }

                        var settingManager = Setting.ClassIntegrationManager.Instance;
                        var integration = settingManager.GetIntegration();

                        // テンプレートのルートディレクトリ
                        ret = integration.TemplateRootDirectory;

                        // ディレクトリの区切りを付加
                        ret = Shared.Tool.ClassPath.DirectoryDelimiter(ret);

                        // 機種名を付加
                        ret += comboboxItemModel.Value;
                    } while (false);

                    return ret;
                }

                /// <summary>
                /// 参照する装置ディレクトリを取得
                /// </summary>
                /// <returns>参照する装置ディレクトリ</returns>
                private string GetSourcePath()
                {
                    string ret = "";

                    // 参照する機種ディレクトリを取得
                    ret = GetModelSourcePath();

                    // ディレクトリの区切りを付加
                    ret = Shared.Tool.ClassPath.DirectoryDelimiter(ret);

                    // 装置名を付加
                    ret += _equipmentSource;

                    return ret;
                }

                /// <summary>
                /// 派生後の装置ディレクトリを取得
                /// </summary>
                /// <returns>派生後の装置ディレクトリ</returns>
                private string GetDestinationPath()
                {
                    string ret = "";

                    // 派生後の機種ディレクトリを取得
                    ret = GetModelSourcePath();

                    do
                    {
                        // 選択しているコンボボックスの項目を取得
                        var comboboxItemModel = comboboxModel.SelectedItem as ComboboxItem.ClassModel;

                        // コンボボックスの項目を確認
                        if (comboboxItemModel == null)
                        {
                            // コンボボックスの項目が未選択 ⇒ 処理を抜ける
                            break;
                        }

                        // 選択しているコンボボックスの項目を取得
                        var comboboxItemEquipmentNumber = comboboxEquipmentNumber.SelectedItem as ComboboxItem.Equipment.ClassNumber;

                        // コンボボックスの項目を確認
                        if (comboboxItemEquipmentNumber == null)
                        {
                            // コンボボックスの項目が未選択 ⇒ 処理を抜ける
                            break;
                        }

                        var settingManager = Setting.ClassIntegrationManager.Instance;
                        var integration = settingManager.GetIntegration();

                        // テンプレートのルートディレクトリ
                        ret = integration.TemplateRootDirectory;

                        // ディレクトリの区切りを付加
                        ret = Shared.Tool.ClassPath.DirectoryDelimiter(ret);

                        // 機種名を付加
                        ret += comboboxItemModel.Value;

                        // ディレクトリの区切りを付加
                        ret = Shared.Tool.ClassPath.DirectoryDelimiter(ret);

                        // 装置名を付加
                        ret = labelEquipmentHeaderDestination.Content + comboboxItemEquipmentNumber.Title + labelEquipmentRevisionDestination.Content;
                    } while (false);

                    return ret;
                }
            }
        }
    }
}
