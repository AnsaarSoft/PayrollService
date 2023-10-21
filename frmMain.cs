using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DIHRMS;
using UFFU;
using DIHRMS.Custom;

namespace PayrollService
{
    public partial class frmMain : Form
    {

        #region Variable

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        string SAPConString;
        string PayrollConString;
        List<string> EmployeeElementList = new List<string>();
        mFm oUtility;

        #endregion

        #region Events


        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                oUtility = new mFm(Application.StartupPath, false, false);
                logger.Info($"System Hardware Key: {mFm.mfmGetSystemID()}");
                if (!ValidateLicense(Properties.Settings.Default.LicenseKey))
                {
                    logger.Info("license expired or invalid.");
                    //Application.Exit();
                }
                lblVersion.Text = $"Version {Application.ProductVersion}";
                tmrEmployeeSync.Interval = Properties.Settings.Default.EmployeeTimer;
                tmrEmployeeSync.Enabled = true;
                //86400000 milisecond = 1 day
                tmrFSEmployee.Interval = Properties.Settings.Default.EmployeeFSTimer;
                tmrFSEmployee.Enabled = true;
                //tmrFSEmployee.Interval = 86400000;
                tmrProbation.Interval = Properties.Settings.Default.PFTimer - 60000;
                tmrProbation.Enabled = true;
                tmrEOBI.Interval = Properties.Settings.Default.EOBITimer - 120000;
                tmrEOBI.Enabled = true;
                SAPConString = Properties.Settings.Default.SapConString;
                PayrollConString = Properties.Settings.Default.PayrollConString;
                logger.Info($"Sap Connection String: {SAPConString}");
                logger.Info($"Payroll Connection String: {PayrollConString}");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void tmrEmployeeSync_Tick(object sender, EventArgs e)
        {
            try
            {
                logger.Info("employee sync start.");
                EmployeeSync();
                //AssignShifts();
                logger.Info("employee sync end.");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void tmrFSEmployee_Tick(object sender, EventArgs e)
        {
            try
            {
                logger.Info("employee fs sync start.");
                ReverseEmployeeSync();
                logger.Info("employee fs sync end.");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void tmrProbation_Tick(object sender, EventArgs e)
        {
            try
            {
                logger.Info("employee probation check start.");
                CheckProbationRoutine();
                logger.Info("employee probation check end.");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void tmrEOBI_Tick(object sender, EventArgs e)
        {
            try
            {
                logger.Info("employee eobi check start.");
                CheckEOBIRoutine();
                logger.Info("employee eobi check end.");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private void cmsExit_Click(object sender, EventArgs e)
        {
            logger.Info($"User Close Application: {DateTime.Now}");
            Application.Exit();
        }
        
        private void btnTest_Click(object sender, EventArgs e)
        {
            //CheckProbationRoutine();
            CheckEOBIRoutine();
        }
        #endregion

        #region Functions

        public void EmployeeSync()
        {
            lblAction.Text = "Processing...";
            wb.Visible = true;
            wb.StartWaiting();
            try
            {
                using (dbHRMS context = new dbHRMS(PayrollConString))
                {
                    EmployeeElementList.Clear();
                    string strGetAllEmployee = $@"
                    SELECT ISNULL(a.empID, '') EmpId,
                       ISNULL(a.firstName, '') firstName,
                       ISNULL(a.lastName, '') lastName,
                       ISNULL(a.sex, '') sex,
                       ISNULL(a.email, '') email,
                       ISNULL(a.jobTitle, '') Designation,
                       ISNULL(a.U_EvalStartDate, '2012-12-12') DOJ,
                       ISNULL(a.birthDate, '2012-12-12') birthDate,
                       ISNULL(a.termDate,'2012-12-12') termdate,
                       a.Active,
                       ISNULL(a.mobile, '') mobile,
                       ISNULL(a.homeTel, '') homeTel,
                       ISNULL(a.govID, '') CNIC,
                       a.salary,
                       ISNULL(bankAcount, '') bankAcount,
                       b.Name AS DeptName,
                       ISNULL(a.status, 0) AS EmployeeStatus,
	                   CAST( ISNULL(a.workStreet,'') AS NVARCHAR(100)) AS WorkStreet,
	                   CAST(ISNULL(a.StreetNoW, '') AS NVARCHAR(100)) AS WorkStreetNo,
	                   CAST(ISNULL(a.workBlock, '') AS NVARCHAR(50)) AS WorkBlock,
	                   CAST( ISNULL(a.WorkBuild,'') AS NVARCHAR(100)) AS WorkBuild,
	                   ISNULL(a.workZip,'') AS WorkZip,
	                   ISNULL(a.workCity,'') AS WorkCity,
	                   CAST(ISNULL(a.homeStreet,'') AS NVARCHAR(100)) AS HomeStreet,
	                   CAST(ISNULL(a.StreetNoH,'') AS NVARCHAR(100)) AS HomeStreetNo,
	                   CAST(ISNULL(a.homeBlock,'') AS NVARCHAR(50)) AS HomeBlock,
	                   CAST(ISNULL(a.HomeBuild,'') AS NVARCHAR(100)) AS HomeBuild,
	                   ISNULL(a.homeZip,'') AS HomeZip,
	                   ISNULL(a.homeCity,'')  AS HomeCity
                FROM dbo.OHEM a
                    INNER JOIN dbo.OUDP b
                        ON a.dept = b.Code                       
                    ";
                    DataTable EmployeeCollection = ExecuteDatatable(strGetAllEmployee, SAPConString);
                    if (EmployeeCollection.Rows.Count > 0)
                    {
                        var DesignationList = (from a in context.MstDesignation
                                               where a.FlgActive == true
                                               select a).ToList();
                        var DepartmentList = (from a in context.MstDepartment
                                              where a.FlgActive == true
                                              select a).ToList();

                        logger.Info($"employees fetched from sapb1. total {EmployeeCollection.Rows.Count}");
                        foreach (DataRow row in EmployeeCollection.Rows)
                        {
                            string empcode = row["EmpId"].ToString();

                            int employeecheck = (from a in context.MstEmployee where a.EmpID == empcode select a).Count();
                            if (employeecheck > 0)
                            {
                                logger.Info($"employee found {empcode}");
                                MstEmployee employee = (from a in context.MstEmployee where a.EmpID == empcode select a).FirstOrDefault();

                                employee.FirstName = row["firstName"].ToString();
                                employee.MiddleName = "";
                                employee.LastName = row["lastName"].ToString();
                                employee.OfficeMobile = Convert.ToString(row["mobile"]);
                                employee.HomePhone = Convert.ToString(row["homeTel"]);
                                employee.OfficeEmail = Convert.ToString(row["email"]);
                                string jobtitle = Convert.ToString(row["Designation"]);
                                var desigvalue = (from a in DesignationList where a.Name == jobtitle select a).FirstOrDefault();
                                if (desigvalue is null)
                                {
                                    employee.DesignationID = (from a in DesignationList select a.Id).FirstOrDefault();
                                    employee.DesignationName = (from a in DesignationList select a.Description).FirstOrDefault();
                                }
                                else
                                {
                                    employee.DesignationID = desigvalue.Id;
                                    employee.DesignationName = desigvalue.Description;
                                }
                                string deptval = Convert.ToString(row["DeptName"]);
                                var deptvalue = (from a in DepartmentList where a.Code == deptval select a).FirstOrDefault();
                                if (deptvalue is null)
                                {
                                    employee.DepartmentID = (from a in DepartmentList select a.ID).FirstOrDefault();
                                    employee.DepartmentName = (from a in DepartmentList select a.DeptName).FirstOrDefault();
                                }
                                else
                                {
                                    employee.DepartmentID = deptvalue.ID;
                                    employee.DepartmentName = deptvalue.DeptName;
                                }
                                DateTime evaldate = Convert.ToDateTime(row["DOJ"]);
                                if (evaldate != new DateTime(2012, 12, 12))
                                {
                                    employee.JoiningDate = evaldate;
                                }
                                DateTime birth = Convert.ToDateTime(row["birthDate"]);
                                if (birth != new DateTime(2012, 12, 12))
                                {
                                    employee.DOB = birth;
                                }
                                DateTime termdate = Convert.ToDateTime(row["termdate"]);
                                if (termdate != new DateTime(2012, 12, 12))
                                {
                                    employee.ResignDate = termdate;
                                    employee.TerminationDate = termdate;
                                }
                                string sex = Convert.ToString(row["sex"]);
                                if (!string.IsNullOrEmpty(sex))
                                {
                                    if (sex == "M")
                                        employee.GenderID = "MALE";
                                    else
                                        employee.GenderID = "FEMALE";
                                }
                                employee.IDNo = Convert.ToString(row["CNIC"]);
                                employee.BasicSalary = Convert.ToDecimal(row["salary"]);
                                string empstatus = Convert.ToString(row["EmployeeStatus"]);
                                if (empstatus == "1")
                                    employee.EmployeeContractType = "CONF";
                                else if (empstatus == "2")
                                    employee.EmployeeContractType = "PROB";
                                else
                                    employee.EmployeeContractType = "-1";
                                employee.AccountNo = Convert.ToString(row["bankAcount"]);
                                employee.FlgActive = Convert.ToString(row["Active"]) == "Y" ? true : false;
                                employee.PayrollID = 1;
                                employee.PayrollName = "Standard AV";
                                employee.Location = 2;
                                employee.LocationName = "Lahore";
                                //employee.FlgSandwich = true;
                                //employee.FlgEmail = true;
                                employee.BranchID = 1;
                                employee.BranchName = "Default";
                                //employee.FlgTax = true;
                                employee.WAStreet = Convert.ToString(row["WorkStreet"]);
                                employee.WAStreetNo = Convert.ToString(row["WorkStreetNo"]);
                                employee.WABlock = Convert.ToString(row["WorkBlock"]);
                                employee.WAOther = Convert.ToString(row["WorkBuild"]);
                                employee.WAZipCode = Convert.ToString(row["WorkZip"]);
                                employee.WACity = Convert.ToString(row["WorkCity"]);
                                employee.HAStreet = Convert.ToString(row["HomeStreet"]);
                                employee.HAStreetNo = Convert.ToString(row["HomeStreetNo"]);
                                employee.HABlock = Convert.ToString(row["HomeBlock"]);
                                employee.HAOther = Convert.ToString(row["HomeBuild"]);
                                employee.HAZipCode = Convert.ToString(row["HomeZip"]);
                                employee.HACity = Convert.ToString(row["HomeCity"]);
                                employee.UpdatedBy = "Auto";
                                employee.UpdateDate = DateTime.Now;
                            }
                            else
                            {
                                logger.Info($"employee not found {empcode}");
                                MstEmployee employee = new MstEmployee();
                                employee.EmpID = empcode;
                                employee.FirstName = row["firstName"].ToString();
                                employee.MiddleName = "";
                                employee.LastName = row["lastName"].ToString();
                                employee.OfficeMobile = Convert.ToString(row["mobile"]);
                                employee.HomePhone = Convert.ToString(row["homeTel"]);
                                employee.OfficeEmail = Convert.ToString(row["email"]);
                                string jobtitle = Convert.ToString(row["Designation"]);
                                var desigvalue = (from a in DesignationList where a.Name == jobtitle select a).FirstOrDefault();
                                if (desigvalue is null)
                                {
                                    employee.DesignationID = (from a in DesignationList select a.Id).FirstOrDefault();
                                    employee.DesignationName = (from a in DesignationList select a.Description).FirstOrDefault();
                                }
                                else
                                {
                                    employee.DesignationID = desigvalue.Id;
                                    employee.DesignationName = desigvalue.Description;
                                }
                                string deptval = Convert.ToString(row["DeptName"]);
                                var deptvalue = (from a in DepartmentList where a.Code == deptval select a).FirstOrDefault();
                                if (deptvalue is null)
                                {
                                    employee.DepartmentID = (from a in DepartmentList select a.ID).FirstOrDefault();
                                    employee.DepartmentName = (from a in DepartmentList select a.DeptName).FirstOrDefault();
                                }
                                else
                                {
                                    employee.DepartmentID = deptvalue.ID;
                                    employee.DepartmentName = deptvalue.DeptName;
                                }
                                DateTime evaldate = Convert.ToDateTime(row["DOJ"]);
                                if (evaldate != new DateTime(2012, 12, 12))
                                {
                                    employee.JoiningDate = evaldate;
                                }
                                DateTime birth = Convert.ToDateTime(row["birthDate"]);
                                if (birth != new DateTime(2012, 12, 12))
                                {
                                    employee.DOB = birth;
                                }
                                DateTime termdate = Convert.ToDateTime(row["termdate"]);
                                if (termdate != new DateTime(2012, 12, 12))
                                {
                                    employee.ResignDate = termdate;
                                    employee.TerminationDate = termdate;
                                }
                                string sex = Convert.ToString(row["sex"]);
                                if (!string.IsNullOrEmpty(sex))
                                {
                                    if (sex == "M")
                                        employee.GenderID = "MALE";
                                    else
                                        employee.GenderID = "FEMALE";
                                }
                                employee.IDNo = Convert.ToString(row["CNIC"]);
                                employee.BasicSalary = Convert.ToDecimal(row["salary"]);
                                string empstatus = Convert.ToString(row["EmployeeStatus"]);
                                if (empstatus == "1")
                                    employee.EmployeeContractType = "CONF";
                                else if (empstatus == "2")
                                    employee.EmployeeContractType = "PROB";
                                else
                                    employee.EmployeeContractType = "-1";
                                employee.AccountNo = Convert.ToString(row["bankAcount"]);
                                employee.FlgActive = Convert.ToString(row["Active"]) == "Y" ? true : false;
                                employee.PayrollID = 1;
                                employee.PayrollName = "Standard AV";
                                employee.Location = 2;
                                employee.LocationName = "Lahore";
                                employee.FlgUser = true;
                                employee.FlgSandwich = true;
                                employee.FlgEmail = true;
                                employee.BranchID = 1;
                                employee.BranchName = "Default";
                                employee.FlgTax = true;
                                employee.WAStreet = Convert.ToString(row["WorkStreet"]);
                                employee.WAStreet = Convert.ToString(row["WorkStreet"]);
                                employee.WAStreetNo = Convert.ToString(row["WorkStreetNo"]);
                                employee.WABlock = Convert.ToString(row["WorkBlock"]);
                                employee.WAOther = Convert.ToString(row["WorkBuild"]);
                                employee.WAZipCode = Convert.ToString(row["WorkZip"]);
                                employee.WACity = Convert.ToString(row["WorkCity"]);
                                employee.HAStreet = Convert.ToString(row["HomeStreet"]);
                                employee.HAStreetNo = Convert.ToString(row["HomeStreetNo"]);
                                employee.HABlock = Convert.ToString(row["HomeBlock"]);
                                employee.HAOther = Convert.ToString(row["HomeBuild"]);
                                employee.HAZipCode = Convert.ToString(row["HomeZip"]);
                                employee.HACity = Convert.ToString(row["HomeCity"]);
                                employee.SortOrder = Convert.ToInt32(empcode) + 50;
                                employee.CreatedBy = employee.UpdatedBy = "Auto";
                                employee.CreateDate = employee.UpdateDate = DateTime.Now;
                                MstUsers oUser = new MstUsers();
                                oUser.UserID = empcode;
                                oUser.UserCode = empcode;
                                oUser.PassCode = "1234";
                                oUser.CreatedBy = oUser.UpdatedBy = "Auto";
                                oUser.CreateDate = oUser.UpdateDate = DateTime.Now;
                                employee.MstUsers.Add(oUser);
                                context.MstEmployee.InsertOnSubmit(employee);
                                EmployeeElementList.Add(empcode);
                            }
                        }
                        context.SubmitChanges();
                        if (EmployeeElementList.Count > 0)
                        {
                            AssignElements();
                        }
                    }
                    else
                    {
                        logger.Info("no employee fetched from sabp1");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            wb.StopWaiting();
            wb.Visible = false;
            lblAction.Text = $"Last Queue run @ {DateTime.Now.ToString("dd/MM/yy hh:mm tt")}";
        }

        public void ReverseEmployeeSync()
        {
            try
            {
                using (dbHRMS context = new dbHRMS(PayrollConString))
                {
                    var olist = (from a in context.MstEmployee
                                 where a.FlgActive == false
                                 select a).ToList();
                    string EmployeeIds = "";
                    if (olist.Count > 0)
                    {
                        foreach (var one in olist)
                        {
                            EmployeeIds += $"{one.EmpID} ,";
                        }
                        //EmployeeIds = EmployeeIds.Remove(EmployeeIds.Length - 2 , 1);
                        string value = EmployeeIds.Substring(0, EmployeeIds.Length - 1);
                        logger.Info($"Employee List {EmployeeIds}");
                        string UpdateQuery = $"Update OHEM Set U_PayrollStatus = 'N' Where empId In ({value})";
                        logger.Info($"Update FS Query: {UpdateQuery}");
                        if (ExecuteNonQuery(UpdateQuery, SAPConString))
                        {
                            logger.Info("Success update FSEmployee");
                        }
                        else
                        {
                            logger.Warn("Error Update FSEmployee.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void AssignElements()
        {
            try
            {
                using (dbHRMS context = new dbHRMS(PayrollConString))
                {
                    //public DataServices ds;
                    //ds = new DataServices(dbHrPayroll, Program.objHrmsUI.HRMSDbName, "");
                    DataServices dataServices = new DataServices(context, context.Connection.Database, "");
                    foreach (string code in EmployeeElementList)
                    {
                        //ds.updateStandardElements(oEmp.EmpID, true, Program.ConStrHRMS);
                        if (string.IsNullOrEmpty(code)) continue;
                        dataServices.updateStandardElements(code, true, PayrollConString);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void CheckProbationRoutine()
        {
            try
            {
                using (dbHRMS context = new dbHRMS(PayrollConString))
                {
                    var oEmployees = (from a in context.MstEmployee
                                      where a.EmployeeContractType == "PROB"
                                      && a.FlgActive == true
                                      select a).ToList();
                    if(oEmployees.Count > 0)
                    {
                        string EmployeeList = "";
                        foreach(var employee in oEmployees)
                        {
                            var oSalaryCount = (from a in context.TrnsSalaryProcessRegister
                                                where a.EmpID == employee.ID
                                                select a).Count();
                            if(oSalaryCount >= 3)
                            {
                                //employee.EmployeeContractType = "CONF";
                                EmployeeList += $"{employee.EmpID} ,";
                            }
                        }
                        string value = EmployeeList.Substring(0, EmployeeList.Length - 1);
                        logger.Info($"Employee List {value}");
                        string UpdateQuery = $"Update OHEM Set status = 1 Where empId In ({value})";
                        logger.Info($"Update Probation Query: {UpdateQuery}");
                        if (ExecuteNonQuery(UpdateQuery, SAPConString))
                        {
                            logger.Info("Success update Probation Employees");
                        }
                        else
                        {
                            logger.Warn("Error Update Probation Employees.");
                        }
                        //context.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public void CheckEOBIRoutine()
        {
            try
            {
                using (dbHRMS context = new dbHRMS(PayrollConString))
                {
                    var oEmployees = (from a in context.MstEmployee 
                                      where a.EmployeeContractType == "CONF"
                                      && a.FlgActive == true
                                      select a).ToList();
                    if (oEmployees.Count > 0)
                    {
                        foreach (var employee in oEmployees)
                        {
                            var oSalaryCount = (from a in context.TrnsSalaryProcessRegister
                                                where a.EmpID == employee.ID
                                                select a).Count();
                            MstElements oMstEobiElement = (from a in context.MstElements
                                                           where a.Id == 2
                                                           select a).FirstOrDefault();
                            if (oSalaryCount >= 5)
                            {
                                var oElementHead = (from a in context.TrnsEmployeeElement
                                                     where a.EmployeeId == employee.ID
                                                     select a).FirstOrDefault();
                                if (oElementHead != null)
                                {
                                    int ElementCount = oElementHead.TrnsEmployeeElementDetail.Where(a => a.ElementId == 2).Count();
                                    if(ElementCount == 0)
                                    {
                                        clsElement oBLElement = new clsElement(context, oMstEobiElement);
                                        TrnsEmployeeElementDetail oElement = new TrnsEmployeeElementDetail();
                                        oElement.MstElements = oMstEobiElement;
                                        oElement.FlgRetro = false;
                                        oElement.RetroAmount = 0;
                                        oElement.StartDate = oMstEobiElement.StartDate;
                                        oElement.EndDate = oMstEobiElement.EndDate;
                                        oElement.ElementType = oMstEobiElement.ElmtType;
                                        oElement.ValueType = oBLElement.ValueType;
                                        oElement.Value = oBLElement.Value;
                                        oElement.Amount = oBLElement.Amount;
                                        oElement.EmpContr = 0;
                                        oElement.EmplrContr = 0;
                                        oElement.FlgActive = true;
                                        oElement.CreateDate = DateTime.Now;
                                        oElement.UpdateDate = DateTime.Now;
                                        oElement.UserId = "Auto";
                                        oElement.UpdatedBy = "Auto";
                                        oElementHead.TrnsEmployeeElementDetail.Add(oElement);
                                        context.SubmitChanges();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        public bool ValidateLicense(string key)
        {
            try
            {
                bool resultValue = false;
                string result = mFm.mfmVerifyLicense(key);
                //result = "Verification Succeded";
                switch (result)
                {
                    case "Verification Succeded":
                        resultValue = true;
                        break;
                    default:
                        resultValue = false;
                        break;
                }
                if (!resultValue)
                {
                    logger.Info("License expires. renew you license.");
                }
                else
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }

        public void AssignShifts()
        {
            lblAction.Text = "Processing...";
            wb.Visible = true;
            wb.StartWaiting();
            Task.Delay(60000);
            try
            {
                using (dbHRMS context = new dbHRMS(PayrollConString))
                {
                    var oEmployeeList = (from a in context.MstEmployee
                                         where (a.FlgActive != null ? a.FlgActive : false) == true
                                         select a).Take(10).ToList();
                    logger.Info($"total employees for shift working. {oEmployeeList.Count}");
                    if (oEmployeeList.Count > 0)
                    {
                        var oCalendar = (from a in context.MstCalendar where a.FlgActive == true select a).FirstOrDefault();
                        foreach (var oEmp in oEmployeeList)
                        {

                            var oPayroll = (from a in context.CfgPayrollDefination where a.ID == oEmp.PayrollID select a).FirstOrDefault();
                            if (oPayroll is null)
                            {
                                logger.Info($"Payroll not define for employee {oEmp.EmpID}");
                                continue;
                            }
                            var oDefaultShift = (from a in context.MstShifts where a.Code == oEmp.WorkIM select a).FirstOrDefault();
                            for (DateTime RunningDay = oCalendar.StartDate.GetValueOrDefault(); RunningDay <= oCalendar.EndDate; RunningDay = DateTime.Now.AddDays(1))
                            {
                                if (oEmp.JoiningDate.GetValueOrDefault() > RunningDay)
                                {
                                    continue;
                                }
                                if(oEmp.TerminationDate.GetValueOrDefault() < RunningDay)
                                {
                                    continue;
                                }
                                var CheckShiftRecord = (from a in context.TrnsAttendanceRegister
                                                        where a.Date == RunningDay.Date
                                                        select a).Count();
                                if (CheckShiftRecord == 0)
                                {
                                    TrnsAttendanceRegister oRecord = new TrnsAttendanceRegister();
                                    oRecord.Date = RunningDay.Date;
                                    oRecord.ShiftID = oDefaultShift.Id;
                                    context.TrnsAttendanceRegister.InsertOnSubmit(oRecord);
                                }
                            }
                            context.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            wb.StopWaiting();
            wb.Visible = false;
            lblAction.Text = $"Last Queue run @ {DateTime.Now.ToString("dd/MM/yy hh:mm tt")}";
        }

        private bool ExecuteNonQuery(string Query, string ConString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(Query, connection);
                    int i = command.ExecuteNonQuery();
                    if (i > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
        }

        private string ExecuteScaler(string Query, string ConString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(Query, connection);
                    string i = command.ExecuteScalar().ToString();
                    return i;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return "Error";
            }
        }

        private DataTable ExecuteDatatable(string Query, string ConString)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConString))
                {
                    connection.Open();
                    SqlDataAdapter da = new SqlDataAdapter(Query, connection);
                    da.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return dt;
            }
        }

        #endregion

    }
}
