#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Services;
using UnityEditor;
using UnityEngine;

namespace ThinIce
{
    [CustomEditor(typeof(GoogleSynchronizer))]
    public class GoogleSheetsHelper : Editor
    {
        static string ApplicationName = "Google Sheets API .NET Quickstart";
        static string SheetId = "1-j1RyxXgwvaPhj9Ykbst4moWltIKFxoHoQU9SRkX2mo";

        private static string CredPath(string appPath) => $"{appPath}\\Credentials\\thinice.json";

        private static SheetsService _service;

        public override void OnInspectorGUI()
        {
            // if (GUILayout.Button("Get new languages"))
            // {
            //     CreateNewLanguages();
            // }

            if (GUILayout.Button("Import"))
            {
                Read();
            }
        }


        private void CreateNewLanguages()
        {
            var cred = GoogleCredential.FromFile(CredPath(Application.dataPath))
                .CreateScoped();
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = cred,
                ApplicationName = ApplicationName
            });
            var path = $"{Application.dataPath}/Scripts/Language.cs";
            var sheets = _service.Spreadsheets.Get(SheetId).Execute().Sheets;
            var sheetNames = sheets.Select(x => x.Properties.Title).Where(x => x.Trim().StartsWith("locale")).ToList();

            File.WriteAllText(path, GetLanguageFile(sheetNames.Select(x => x.Split(' ').LastOrDefault())));
            EditorUtility.RequestScriptReload();
        }

        private void Read()
        {
            var cred = GoogleCredential.FromFile(CredPath(Application.dataPath))
                .CreateScoped();
            _service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = cred,
                ApplicationName = ApplicationName
            });
            var dialog = FindObjectOfType<GameDialogWithLanguages>(true);
            dialog.localizedDialogCommons = new List<LanguageDialogCommon>();
            dialog.startGuyText = new List<LocalizedText>();
            dialog.totalFailedText = new List<LocalizedText>();
            var sheets = _service.Spreadsheets.Get(SheetId).Execute().Sheets;
            var sheetNames = sheets.Select(x => x.Properties.Title).Where(x => x.Trim().StartsWith("locale")).ToList();

            var uiElements = FindObjectsOfType<UiLocalizedItem>(true);
            foreach (var uiElement in uiElements)
                uiElement.LocalizedTexts = new List<LocalizedText>();

            foreach (var sheetName in sheetNames)
            {
                ImportNovell(dialog, sheetName);
                ImportAnotherNovellTexts(dialog, sheetName);
                ImportUiTexts(uiElements, sheetName);
            }
        }

        private void ImportNovell(GameDialogWithLanguages dialog, string sheetName)
        {
            var range = $"{sheetName}!B4:J12";

            var dialogStateKeeper = FindObjectOfType<DialogStateKeeper>(true);

            var req = _service.Spreadsheets.Values.Get(SheetId, range);

            var res = req.Execute();
            uint id = 0;

            if (!Enum.TryParse<Language>(sheetName.Split(' ').LastOrDefault(), out var langType)) return;

            var dialogCommons = new LanguageDialogCommon
            {
                Language = langType,
                DialogCommons = new List<DialogCommon>()
            };

            dialog.localizedDialogCommons.Add(dialogCommons);
            if (res.Values != null)
            {
                foreach (var column in res.Values)
                {
                    var rowCount = column.Count;
                    if (rowCount < 1)
                        continue;
                    var dialogCommon = new DialogCommon
                    {
                        textGirl = column[0].ToString(),
                        textGuy = new TextGuy[4],
                    };

                    for (int i = 1, j = 0; i < column.Count; i += 2, j++)
                    {
                        var dialogState = dialogStateKeeper.DialogStateDict[id];
                        dialogCommon.textGuy[j] = new TextGuy
                        {
                            id = id,
                            text = column[i].ToString(),
                            girlAnswer = column[i + 1].ToString(),
                            badText = dialogState.IsBadText,
                            girtAnswerEmotion = dialogState.Emotion
                        };
                        id++;
                    }

                    dialogCommons.DialogCommons.Add(dialogCommon);
                    dialogCommon.girlEmotion =
                        dialogStateKeeper.MainDialogEmotions[dialogCommons.DialogCommons.Count - 1];
                }
            }
        }

        private void ImportAnotherNovellTexts(GameDialogWithLanguages dialog, string sheetName)
        {
            var range = $"{sheetName}!B16:C17";

            var req = _service.Spreadsheets.Values.Get(SheetId, range);

            var res = req.Execute();

            if (!Enum.TryParse<Language>(sheetName.Split(' ').LastOrDefault(), out var langType)) return;
            if (res.Values != null)
            {
                foreach (var column in res.Values)
                {
                    var rowCount = column.Count;
                    if (rowCount < 1)
                        continue;

                    var key = column[0].ToString();
                    var value = column[1].ToString();
                    if (key == "novell_man_first_frase")
                        dialog.startGuyText.Add(new LocalizedText
                        {
                            Language = langType,
                            Text = value
                        });

                    if (key == "novell_bad_end_woman_frase")
                        dialog.totalFailedText.Add(new LocalizedText
                        {
                            Language = langType,
                            Text = value
                        });
                }
            }
        }

        private void ImportUiTexts(UiLocalizedItem[] uiElements, string sheetName)
        {
            var range = $"{sheetName}!H16:I70";

            var req = _service.Spreadsheets.Values.Get(SheetId, range);

            var res = req.Execute();

            if (!Enum.TryParse<Language>(sheetName.Split(' ').LastOrDefault(), out var langType)) return;
            if (res.Values != null)
            {
                foreach (var column in res.Values)
                {
                    var rowCount = column.Count;
                    if (rowCount < 1)
                        continue;

                    var key = column[0].ToString();
                    var value = column[1].ToString();
                    if (string.IsNullOrEmpty(key)) continue;

                    var foundUiItems = uiElements.Where(x => x.Key == key);
                    foreach (var foundUiItem in foundUiItems)
                    {
                        foundUiItem.LocalizedTexts.Add(new LocalizedText
                        {
                            Language = langType,
                            Text = value
                        });
                    }
                }
            }
        }

        private string GetLanguageFile(IEnumerable<string> languages)
        {
            return @$"namespace ThinIce
{{
    public enum Language
    {{
        {string.Join(",\n\t\t", languages)}
    }}
}}";
        }
    }
}
#endif