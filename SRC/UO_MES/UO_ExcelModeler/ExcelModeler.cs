using System;
using System.Collections;
using System.Reflection;
using Aspose.Cells;
using UO_Service.Base;
using System.Runtime.InteropServices;
using System.Globalization;

namespace UO_ExcelModeler
{
    [ComVisible(false)]
    public class ExcelModeler
    {
        public bool CleanToInsertStatus(string model_file_path)
        {
            Workbook workBook = new Workbook();
            workBook.Open(model_file_path);

            foreach (Worksheet workSheet in workBook.Worksheets)
            {
                Cells cls = workSheet.Cells;
                string baseType = cls[0, 1].Value.ToString();

                switch (baseType)
                {
                    case "Named object:":
                    case "Revisioned object:":
                        CleanExecuteAndCompleteMsg(cls);
                        break;
                }
            }

            workBook.Save(model_file_path);
            return true;
        }

        private void CleanExecuteAndCompleteMsg(Cells cls)
        {
            Cell firstExecuteCell = cls.FindString("Execute", null);
            int startRow = firstExecuteCell.Row + 1;
            do
            {
                Cell executeActionCell = cls[startRow, 0];
                if (executeActionCell.Value == null)
                    break;
                Cell completeMsgCell = cls[startRow, 1];

                executeActionCell.PutValue("INSERT");
                completeMsgCell.PutValue("");
            } while (cls[++startRow, 0].Value != null);
        }

        public bool Import(string model_file_path)
        {
            Workbook workBook = new Workbook();
            workBook.Open(model_file_path);

            foreach (Worksheet workSheet in workBook.Worksheets)
            {
                Cells cls = workSheet.Cells;
                string baseType = cls[0, 1].Value.ToString();
                string className = cls[1, 1].Value.ToString();

                switch (baseType)
                {
                    case "Named object:":
                        ImportNamedObjects(cls, className);
                        break;
                    case "Revisioned object:":
                        ImportRevisionedObjects(cls, className);
                        break;
                }
            }

            workBook.Save(model_file_path);
            return true;
        }

        private void ImportNamedObjects(Cells cls, string ndoClassName)
        {
            Type classType = model_dll.GetType(ndoClassName);
            using (NamedObjectMaint mt = GetNamedObjectMaint(ndoClassName))
            {
                Cell firstExecuteCell = cls.FindString("Execute", null);
                int startRow = firstExecuteCell.Row + 1;
                do
                {
                    Cell executeActionCell = cls[startRow, 0];
                    if (executeActionCell.Value == null)
                        break;
                    string executeAction = executeActionCell.Value.ToString();
                    Cell completeMsgCell = cls[startRow, 1];
                    int startColumn = 2;    // Column 0 is Execute Action, 1 is CompleteMsg
                    switch (executeAction.ToUpper())
                    {
                        case "INSERT":
                            object o_insert = Activator.CreateInstance(classType);
                            fillClassTypeProperty(cls, firstExecuteCell, startRow, startColumn, classType, ref o_insert, mt);
                            UpdateRecordStatus(executeActionCell, completeMsgCell, mt.Add(o_insert as UO_Model.Base.NamedObject));
                            break;
                        case "UPDATE":
                            object o_update = Activator.CreateInstance(classType);
                            fillClassTypeProperty(cls, firstExecuteCell, startRow, startColumn, classType, ref o_update, mt);
                            UpdateRecordStatus(executeActionCell, completeMsgCell, mt.Update(o_update as UO_Model.Base.NamedObject));
                            break;
                        case "REMOVE":
                        case "DELETE":
                            object o_remove = Activator.CreateInstance(classType);
                            fillClassTypeProperty(cls, firstExecuteCell, startRow, startColumn, classType, ref o_remove, mt);
                            UpdateRecordStatus(executeActionCell, completeMsgCell, mt.Remove(o_remove as UO_Model.Base.NamedObject));
                            break;
                        case "IGNORE":
                        default:
                            break;
                    }
                } while (cls[++startRow, 0].Value != null);
            }
        }

        private void ImportRevisionedObjects(Cells cls, string rdoClassName)
        {
            Type classType = model_dll.GetType(rdoClassName);
            using (RevisionedObjectMaint mt = GetRevisionedObjectMaint(rdoClassName))
            {
                Cell firstExecuteCell = cls.FindString("Execute", null);
                int startRow = firstExecuteCell.Row + 1;
                do
                {
                    Cell executeActionCell = cls[startRow, 0];
                    if (executeActionCell.Value == null)
                        break;
                    string executeAction = executeActionCell.Value.ToString();
                    Cell completeMsgCell = cls[startRow, 1];
                    int startColumn = 3;    // Column 0 is Execute Action, 1 is CompleteMsg, 2 is Name
                    switch (executeAction.ToUpper())
                    {
                        case "INSERT":
                            object o_insert = Activator.CreateInstance(classType);
                            SetRevBase(cls[startRow, 2].Value.ToString(), rdoClassName, classType, ref o_insert);
                            fillClassTypeProperty(cls, firstExecuteCell, startRow, startColumn, classType, ref o_insert, mt);
                            UpdateRecordStatus(executeActionCell, completeMsgCell, mt.Add(o_insert as UO_Model.Base.RevisionedObject));
                            break;
                        case "UPDATE":
                            object o_update = Activator.CreateInstance(classType);
                            SetRevBase(cls[startRow, 2].Value.ToString(), rdoClassName, classType, ref o_update);
                            fillClassTypeProperty(cls, firstExecuteCell, startRow, startColumn, classType, ref o_update, mt);
                            UpdateRecordStatus(executeActionCell, completeMsgCell, mt.Update(o_update as UO_Model.Base.RevisionedObject));
                            break;
                        case "REMOVE":
                        case "DELETE":
                            object o_remove = Activator.CreateInstance(classType);
                            SetRevBase(cls[startRow, 2].Value.ToString(), rdoClassName, classType, ref o_remove);
                            fillClassTypeProperty(cls, firstExecuteCell, startRow, startColumn, classType, ref o_remove, mt);
                            UpdateRecordStatus(executeActionCell, completeMsgCell, mt.Remove(o_remove as UO_Model.Base.RevisionedObject));
                            break;
                        case "IGNORE":
                        default:
                            break;
                    }
                } while (cls[++startRow, 0].Value != null);
            }
        }

        private void SetRevBase(string revBaseName, string rdoClassName, Type classType, ref object o_insert)
        {
            // Set RevBase based on Name
            Type classTypeBase = model_dll.GetType(rdoClassName + "Base");
            object o_insert_base = Activator.CreateInstance(classTypeBase);
            PropertyInfo pi_oi_base = classTypeBase.GetProperty("Name");
            pi_oi_base.SetValue(o_insert_base, revBaseName, null);
            PropertyInfo pi_oi_RevBase = classType.GetProperty("RevBase");
            pi_oi_RevBase.SetValue(o_insert, o_insert_base, null);
        }

        private void fillClassTypeProperty(Cells cls, Cell executeCell, int startRow, int startColumn, Type classType, ref object o_insert, Service mt)
        {
            do
            {
                bool classIsList = false;
                if (cls[executeCell.Row - 2, startColumn].Value != null &&
                    cls[executeCell.Row - 2, startColumn].Value.ToString() == "List")
                    classIsList = true;
                string classPropertyType = cls[executeCell.Row - 1, startColumn].Value.ToString();
                string classProperty = cls[executeCell.Row, startColumn].Value.ToString();
                Cell classPropertyValueCell = cls[startRow, startColumn];
                string[] classPropertyValues = { null };
                if (classPropertyValueCell.Value != null)
                {
                    classPropertyValues = classPropertyValueCell.Value.ToString().Split(',');
                }

                PropertyInfo pi_o = classType.GetProperty(classProperty);
                if(null == pi_o)
                    throw new ExcelModelerException(string.Format(ResX.Type_is_unknown, classProperty, classType.Name));

                for (int i = 0; i < classPropertyValues.Length; i++)
                {
                    string obj_value = classPropertyValues[i];

                    switch (classPropertyType)
                    {
                        case "String":
                            if (null != obj_value)
                                InsertByType(ref o_insert, classIsList, pi_o, obj_value);
                            break;
                        case "Boolean":
                            if (null != obj_value)
                            {
                                bool bv = bool.Parse(obj_value);
                                InsertByType(ref o_insert, classIsList, pi_o, bv);
                            }
                            break;
                        case "Integer":
                            if (null != obj_value)
                            {
                                int iv;
                                if (int.TryParse(obj_value, out iv) == true)
                                    InsertByType(ref o_insert, classIsList, pi_o, iv);
                            }
                            break;
                        case "DateTime":
                            if (null != obj_value)
                            {
                                DateTime dv;
                                if (DateTime.TryParse(obj_value, out dv) == true)
                                    InsertByType(ref o_insert, classIsList, pi_o, dv);
                            }
                            break;
                        default:    // all not match type is consider as Object
                            if (null != obj_value)
                                InsertByType(ref o_insert, classIsList, pi_o, mt.ResolveCDO(classPropertyType, obj_value, true));
                            break;
                    }
                }
            } while (cls[executeCell.Row, ++startColumn].Value != null);
        }

        private void InsertByType(ref object o_insert, bool classIsList, PropertyInfo pi_o, object obj_value)
        {
            if (classIsList == false)
                pi_o.SetValue(o_insert, obj_value, null);
            else
            {
                IList vl = pi_o.GetValue(o_insert, null) as IList;
                vl.Add(obj_value);
            }
        }

        private void UpdateRecordStatus(Cell executeActionCell, Cell completeMsgCell, bool res)
        {
            if (res == true)
            {
                completeMsgCell.PutValue(string.Format(ResX.Service_success, executeActionCell.Value.ToString(), DateTime.Now));
                executeActionCell.PutValue("IGNORE");
            }
            else
                completeMsgCell.PutValue(string.Format(ResX.Service_failed, executeActionCell.Value.ToString(), DateTime.Now));
        }

        private NamedObjectMaint GetNamedObjectMaint(string fullClassName)
        {
            string maintClassName = GetMaintClassName(fullClassName);
            Type classType = service_dll.GetType(maintClassName);
            if (classType != null)
                return Activator.CreateInstance(classType) as NamedObjectMaint;
            else
                return new NamedObjectMaint();
        }

        private RevisionedObjectMaint GetRevisionedObjectMaint(string fullClassName)
        {
            string maintClassName = GetMaintClassName(fullClassName);
            Type classType = service_dll.GetType(maintClassName);
            if (classType != null)
                return Activator.CreateInstance(classType) as RevisionedObjectMaint;
            else
                return new RevisionedObjectMaint();
        }

        private string GetMaintClassName(string fullClassName)
        {
            return "UO_Service.Maint." + fullClassName.Substring(fullClassName.LastIndexOf('.') + 1) + "Maint";
        }

        public ExcelModeler(string modelDll, string serviceDll)
        {
            model_dll = Assembly.LoadFrom(modelDll);
            service_dll = Assembly.LoadFrom(serviceDll);
        }

        private Assembly model_dll;
        private Assembly service_dll;
        public static string regSettingPath = @"SOFTWARE\UO-MES\ExcelModeler";
        public static string traceCategory = "UO_ExcelModeler";
    }
}
