using SAPbobsCOM;
using SAPbouiCOM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Metadata
{
    public static class MDResources
    {
        private static SAPbobsCOM.Company _company;

        private static SAPbouiCOM.Application _application;

        private static Version _version;

        private static string _addonID;

        private static string _tableSys;

        private static Action<string, MessageType> _messages;

        private static string _appPath = System.Windows.Forms.Application.StartupPath;

        public static Action<string, MessageType> Messages
        {
            set
            {
                _messages = value;
            }
        }

        public static bool loadMetaData(Version version, SAPbouiCOM.Application application, string companyPrefix, string addonID)
        {
            _company = (SAPbobsCOM.Company)application.Company.GetDICompany();
            _application = application;
            _version = version;
            _addonID = addonID;
            _tableSys = "EXX_SETUP";
            bool result = true;
            string text = "";
            try
            {
                if (createSYSTable())
                {
                    Recordset recordset = (Recordset)_company.GetBusinessObject(BoObjectTypes.BoRecordset);
                    string queryStr = "select * from \"@EXX_SETUP\" where \"U_EXX_ADDN\" = '" + addonID + "'";
                    recordset.DoQuery(queryStr);
                    if (!recordset.EoF)
                    {
                        text = recordset.Fields.Item("U_EXX_VERS").Value.ToString().Trim();
                    }

                    Marshal.ReleaseComObject(recordset);
                    recordset = null;
                    GC.Collect();
                    if (version.ToString() != text)
                    {
                        showWaitingForm();
                        createElements();
                        updateSYSTable(addonID);
                        closeWaitingForm();
                    }
                }
            }
            catch
            {
                throw;
            }

            return result;
        }

        private static void showWaitingForm()
        {
            try
            {
                _application.MetadataAutoRefresh = false;
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(Path.Combine(_appPath, "Resources", "Gif", "Wait.srf"));
                FormCreationParams formCreationParams = (FormCreationParams)_application.CreateObject(BoCreatableObjectType.cot_FormCreationParams);
                formCreationParams.XmlData = xmlDocument.InnerXml;
                formCreationParams.UniqueID = "FrmWt";
                formCreationParams.FormType = "FrmWt";
                SAPbouiCOM.Form form = _application.Forms.AddEx(formCreationParams);
                form.Visible = true;
                Item item = form.Items.Add("WebBrowser", BoFormItemTypes.it_WEB_BROWSER);
                item.Left = 20;
                item.Top = 20;
                item.Width = 200;
                item.Height = 200;
                SAPbouiCOM.WebBrowser webBrowser = (SAPbouiCOM.WebBrowser)item.Specific;
                webBrowser.Url = Path.Combine(_appPath, "Resources", "Gif", "gif.html");
            }
            catch (Exception)
            {
                throw;
            }
        }


        private static void closeWaitingForm()
        {
            try
            {
                SAPbouiCOM.Form form = _application.Forms.Item("FrmWt");
                form.Close();
                _application.MetadataAutoRefresh = true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static bool createSYSTable()
        {
            UserTablesMD userTablesMD = (UserTablesMD)_company.GetBusinessObject(BoObjectTypes.oUserTables);
            if (userTablesMD.GetByKey(_tableSys))
            {
                return true;
            }

            _messages?.Invoke("Creando tabla setup", MessageType.Info);
            userTablesMD.TableName = _tableSys;
            userTablesMD.TableDescription = "Setup de AddOns de EXX";
            userTablesMD.TableType = BoUTBTableType.bott_NoObject;
            if (userTablesMD.Add() != 0)
            {
                throw new InvalidOperationException($"Setup - TBL:{_company.GetLastErrorCode()} - {_company.GetLastErrorDescription()}");
            }

            userTablesMD.Update();
            Marshal.ReleaseComObject(userTablesMD);
            userTablesMD = null;
            GC.Collect();
            UserFieldsMD userFieldsMD = (UserFieldsMD)_company.GetBusinessObject(BoObjectTypes.oUserFields);
            userFieldsMD.TableName = _tableSys;
            userFieldsMD.Name = "EXX_ADDN";
            userFieldsMD.Description = "Nombre del AddOn";
            userFieldsMD.Type = BoFieldTypes.db_Alpha;
            userFieldsMD.Size = 100;
            userFieldsMD.EditSize = 100;
            if (userFieldsMD.Add() != 0)
            {
                throw new InvalidOperationException($"FLD - ADDN: {_company.GetLastErrorCode()} - {_company.GetLastErrorDescription()}");
            }

            userFieldsMD.TableName = _tableSys;
            userFieldsMD.Name = "EXX_VERS";
            userFieldsMD.Description = "Version del AddOn";
            userFieldsMD.Type = BoFieldTypes.db_Alpha;
            userFieldsMD.Size = 100;
            userFieldsMD.EditSize = 100;

            if (userFieldsMD.Add() != 0)
            {
                throw new InvalidOperationException($"FLD - VERS: {_company.GetLastErrorCode()} - {_company.GetLastErrorDescription()}");
            }

            userFieldsMD.TableName = _tableSys;
            userFieldsMD.Name = "EXX_RUTA";
            userFieldsMD.Description = "Ruta auxiliar para AddOn";
            userFieldsMD.Type = BoFieldTypes.db_Alpha;
            userFieldsMD.Size = 254;
            userFieldsMD.EditSize = 254;
            if (userFieldsMD.Add() != 0)
            {
                throw new InvalidOperationException($"FLD - RUTA: {_company.GetLastErrorCode()} - {_company.GetLastErrorDescription()}");
            }

            userFieldsMD.Update();
            Marshal.ReleaseComObject(userFieldsMD);
            userFieldsMD = null;
            GC.Collect();
            return true;
        }

        private static void updateSYSTable(string addonID)
        {
            UserTable userTable = _company.UserTables.Item(_tableSys);
            Recordset recordset = _company.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            string text = "select * from \"@EXX_SETUP\"";
            recordset.DoQuery(text);
            int recordCount = recordset.RecordCount;
            text = text + " where \"U_EXX_ADDN\" = '" + addonID + "'";
            recordset.DoQuery(text);
            if (!recordset.EoF)
            {
                if (userTable.GetByKey(recordset.Fields.Item("Code").Value.ToString()))
                {
                    userTable.UserFields.Fields.Item("U_EXX_ADDN").Value = addonID;
                    userTable.UserFields.Fields.Item("U_EXX_VERS").Value = _version.ToString();
                    if (userTable.Update() != 0)
                    {
                        throw new InvalidOperationException($"SYS - TBL:{_company.GetLastErrorCode()} - {_company.GetLastErrorDescription()}");
                    }
                }
            }
            else
            {
                userTable.Code = (recordCount + 1).ToString().PadLeft(2, '0');
                userTable.Name = (recordCount + 1).ToString().PadLeft(2, '0');
                userTable.UserFields.Fields.Item("U_EXX_ADDN").Value = addonID;
                userTable.UserFields.Fields.Item("U_EXX_VERS").Value = _version.ToString();
                if (userTable.Add() != 0)
                {
                    throw new InvalidOperationException($"SYS - TBL:{_company.GetLastErrorCode()} - {_company.GetLastErrorDescription()}");
                }
            }
        }

        private static void createElements()
        {
            string UTPath = string.Empty;
            string UFPath = string.Empty;
            string UDOPath = string.Empty;
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(_appPath, "Resources", "BO"));
                FileInfo[] files = directoryInfo.GetFiles("*.xml");
                _messages?.Invoke("Iniciando la creacion de elementos", MessageType.Info);
                files.ToList().ForEach(delegate (FileInfo f)
                {
                    if (f.Name.Equals("UT.xml"))
                    {
                        UTPath = f.FullName;
                    }

                    if (f.Name.Equals("UF.xml"))
                    {
                        UFPath = f.FullName;
                    }

                    if (f.Name.Equals("UO.xml"))
                    {
                        UDOPath = f.FullName;
                    }
                });
                //if (UTPath != string.Empty)
                //{
                //    addElements(UTPath);
                //}

                //if (UFPath != string.Empty)
                //{
                //    addElements(UFPath);
                //}

                //if (UDOPath != string.Empty)
                //{
                //    addElements(UDOPath);
                //}

                createStoreProcedures();
            }
            catch
            {
                closeWaitingForm();
                throw;
            }
        }


        private static void addElements(string uTPath)
        {
            int num = 0;
            int num2 = 0;
            num = _company.GetXMLelementCount(uTPath);
            dynamic val = _company.GetXMLobjectType(uTPath, 0).ToString();
            object obj = val;
            object obj2 = obj;
            switch (obj2 as string)
            {
                case "oUserTables":
                    val = "Tablas de usuario";
                    _messages?.Invoke("Creando tablas de usuario", MessageType.Info);
                    break;
                case "oUserFields":
                    val = "Campos de usuario";
                    _messages?.Invoke("Creando campos de usuario", MessageType.Info);
                    break;
                case "oUserObjects":
                    val = "Objetos de usuario";
                    _messages?.Invoke("Creando Objetos de usuario", MessageType.Info);
                    break;
            }

            for (int i = 0; i < num; i++)
            {
                try
                {
                    dynamic businessObjectFromXML = _company.GetBusinessObjectFromXML(uTPath, i);
                    if (businessObjectFromXML.Add() != 0)
                    {
                        _company.GetLastError(out var errCode, out var errMsg);
                        if (errCode != -2035 && errCode != -5002)
                        {
                            num2++;
                            throw new Exception($"{errCode} - {errMsg}");
                        }
                    }

                    Marshal.ReleaseComObject(businessObjectFromXML);
                    GC.Collect();
                }
                catch (Exception ex)
                {
                    closeWaitingForm();
                    _messages?.Invoke(ex.Message, MessageType.Error);
                }
                finally
                {
                    dynamic businessObjectFromXML = null;
                }
            }

            _messages?.Invoke("Fin de la creación de " + val + ". Errores: " + num2, MessageType.Success);
        }

        private static void addTableRecords()
        {
            UserTable userTable = null;
            Recordset recordset = null;
            XElement xElement = null;
            IEnumerable<XElement> enumerable = null;
            IEnumerable<XElement> enumerable2 = null;
            string text = string.Empty;
            string empty = string.Empty;
            string empty2 = string.Empty;
            try
            {
                recordset = (Recordset)_company.GetBusinessObject(BoObjectTypes.BoRecordset);
                empty = Path.Combine(_appPath, "BO", "UR.vte");
                xElement = XElement.Load(empty);
                enumerable = from n in xElement.Elements().Elements()
                             where n.Name.LocalName != "AdmInfo"
                             select n;
                foreach (XElement item in enumerable)
                {
                    if (text != item.Name.LocalName)
                    {
                        empty2 = "DELETE FROM \"@" + item.Name.LocalName + "\"";
                        recordset.DoQuery(empty2);
                        text = item.Name.LocalName;
                    }

                    userTable = _company.UserTables.Item(item.Name.LocalName);
                    enumerable2 = item.Elements().Elements();
                    userTable.Code = enumerable2.ElementAt(0).Value;
                    userTable.Name = enumerable2.ElementAt(1).Value;
                    enumerable2 = enumerable2.Where((XElement n) => n.Name.LocalName != "Code" && n.Name.LocalName != "Name");
                    foreach (XElement item2 in enumerable2)
                    {
                        userTable.UserFields.Fields.Item(item2.Name.LocalName).Value = item2.Value;
                    }

                    if (userTable.Add() != 0)
                    {
                        throw new Exception($"{_company.GetLastErrorCode()} - {_company.GetLastErrorDescription()}");
                    }
                }
            }
            catch
            {
                closeWaitingForm();
                throw;
            }
        }


        private static void createStoreProcedures()
        {
            Recordset recordset = null;
            Recordset recordset2 = null;
            string[] array = null;
            string empty = string.Empty;
            string empty2 = string.Empty;
            string text = string.Empty;
            string empty3 = string.Empty;
            StreamReader streamReader = null;
            string empty4 = string.Empty;
            string[] array2 = null;
            recordset = (Recordset)_company.GetBusinessObject(BoObjectTypes.BoRecordset);
            recordset2 = (Recordset)_company.GetBusinessObject(BoObjectTypes.BoRecordset);
            array = ((_company.DbServerType != BoDataServerTypes.dst_HANADB) 
                ? Directory.GetFiles(Path.Combine(_appPath, "Resources", "Scripts", "SQL"), "*.sql") 
                : Directory.GetFiles(Path.Combine(_appPath, "Resources", "Scripts", "HANA"), "*.sql"));
            for (int i = 0; i < array.GetUpperBound(0) + 1; i++)
            {
                streamReader = new StreamReader(array[i]);
                empty4 = streamReader.ReadToEnd();
                array2 = empty4.Substring(0, 50).Split(' ');
                empty3 = Path.GetFileName(array[i]);
                empty3 = empty3.Substring(0, empty3.Length - 4);

                if (array2[1].Trim().Equals("PROCEDURE"))
                {
                    empty2 = "el procedimiento ";
                    text = "= 'P'";
                }
                else if (array2[1].Trim().Equals("VIEW"))
                {
                    empty2 = "la vista ";
                    text = "= 'V'";
                }
                else if (array2[1].Trim().Equals("FUNCTION"))
                {
                    empty2 = "la funcion ";
                    text = "in (N'FN', N'IF', N'TF', N'FS', N'FT')";
                }

                empty = ((_company.DbServerType != BoDataServerTypes.dst_HANADB) ? ("SELECT COUNT(*) FROM sys.all_objects WHERE type " + text + " and name = '" + empty3.Trim().ToUpper() + "'") : ("SELECT COUNT('A') FROM \"SYS\".\"OBJECTS\" WHERE UPPER(\"OBJECT_NAME\") ='" + empty3.Trim().ToUpper() + "' AND \"SCHEMA_NAME\" = '" + _company.CompanyDB + "'"));
                recordset.DoQuery(empty);
                if (recordset.EoF)
                {
                    continue;
                }

                if (Convert.ToInt32(recordset.Fields.Item(0).Value) != 0)
                {
                    try
                    {
                        empty = "DROP " + array2[1].Trim() + " " + empty3.ToUpper();
                        recordset.DoQuery(empty);
                        recordset.DoQuery(empty4);
                    }
                    catch (Exception ex)
                    {
                        closeWaitingForm();
                        _messages?.Invoke(ex.Message + " " + empty3.ToUpper(), MessageType.Error);
                    }
                }
                else
                {
                    try
                    {
                        recordset.DoQuery(empty4);
                    }
                    catch (Exception ex2)
                    {
                        closeWaitingForm();
                        _messages?.Invoke(ex2.Message +" "+ empty3.ToUpper(), MessageType.Error);
                        var x = empty4;
                    }
                }
            }
        }
    }
}
