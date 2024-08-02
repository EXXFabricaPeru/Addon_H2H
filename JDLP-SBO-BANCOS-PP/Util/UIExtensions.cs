using SAPbobsCOM;
using SAPbouiCOM;

namespace JDLP_SBO_BANCOS_PP.Util
{
    public static class UIExtensions
    {
        public static ComboBox LoadValidValues(this ComboBox comboBox, Recordset recordSet)
        {
            while (comboBox.ValidValues.Count > 0)
            {
                comboBox.ValidValues.Remove(0, BoSearchKey.psk_Index);
            }

            while (!recordSet.EoF)
            {
                comboBox.ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());
                recordSet.MoveNext();
            }

            return comboBox;
        }

        public static ComboBox LoadValidValues(this ComboBox comboBox, Recordset recordSet, object value, object description)
        {
            while (comboBox.ValidValues.Count > 0)
            {
                comboBox.ValidValues.Remove(0, BoSearchKey.psk_Index);
            }

            while (!recordSet.EoF)
            {
                comboBox.ValidValues.Add(recordSet.Fields.Item(value).Value.ToString(), recordSet.Fields.Item(description).Value.ToString());
                recordSet.MoveNext();
            }

            return comboBox;
        }

        public static Column LoadValidValues(this Column colMatrix, Recordset recordSet)
        {
            while (colMatrix.ValidValues.Count > 0)
            {
                colMatrix.ValidValues.Remove(0, BoSearchKey.psk_Index);
            }

            while (!recordSet.EoF)
            {
                colMatrix.ValidValues.Add(recordSet.Fields.Item(0).Value.ToString(), recordSet.Fields.Item(1).Value.ToString());
                recordSet.MoveNext();
            }

            return colMatrix;
        }

        public static DBDataSource SetValueExt(this DBDataSource dbs, object index, string newValue, int recordNumber = 0)
        {
            dbs.SetValue(index, recordNumber, newValue);
            return dbs;
        }

        public static DBDataSource GetDBDataSource(this IForm form, object idDataSource)
        {
            return form.DataSources.DBDataSources.Item(idDataSource);
        }

        public static void SetStatusErrorMessage(this Application application, string message)
        {
            application.StatusBar.SetText(message, BoMessageTime.bmt_Short);
        }

        public static void SetStatusWarningMessage(this Application application, string message)
        {
            application.StatusBar.SetText(message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Warning);
        }

        public static void SetStatusSuccessMessage(this Application application, string message)
        {
            application.StatusBar.SetText(message, BoMessageTime.bmt_Short, BoStatusBarMessageType.smt_Success);
        }

        public static string GetValueExt(this DBDataSource dBDataSource, object index, int recNum = 0)
        {
            return dBDataSource.GetValue(index, recNum);
        }
    }
}
