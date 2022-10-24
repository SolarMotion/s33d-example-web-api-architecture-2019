using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Library.Models;

namespace Library
{
    public static class DBs
    {
        #region Checking

        public static bool IsValidOrg(CommissionIncentiveEntities db, Guid orgGuid)
        {
            return db.tblOrganizations.Any(a => a.GUID == orgGuid && a.IsActive);
        }

        public static bool IsValidFormulaCode(CommissionIncentiveEntities db, string formulaCode)
        {
            return db.refCommissionFormulas.Any(a => a.Code == formulaCode && a.IsActive);
        }

        public static bool HasFoumula(CommissionIncentiveEntities db, Guid orgGuid, string formulaCode)
        {
            return (from a in db.tblOrganizations
                    join b in db.lnkCommissionFormulaOrganizations on a.ID equals b.OrgID
                    join c in db.refCommissionFormulas on b.CommissionFormulaID equals c.ID
                    join d in db.lnkCommissionFormulas on c.ID equals d.CommissionFormulaID
                    join e in db.refCommissionRateTypes on d.CommissionRateTypeID equals e.ID
                    where a.IsActive && a.GUID == orgGuid && b.IsActive && c.IsActive && d.IsActive && e.IsActive && c.Code == formulaCode
                    select new
                    {
                        a.ID
                    }).Any();
        }

        #endregion

        #region Get

        public static tblCommissionBalance GetCommissionBalance(CommissionIncentiveEntities db, int orgID)
        {
            return db.tblCommissionBalances.FirstOrDefault(a => a.OrgID == orgID && a.IsActive);
        }

        public static tblOrganization GetOrg(CommissionIncentiveEntities db, Guid orgGuid)
        {
            return db.tblOrganizations.FirstOrDefault(a => a.GUID == orgGuid && a.IsActive);
        }

        public static List<LibraryCommissionFormulaDetails> GetFormulas(CommissionIncentiveEntities db, Guid orgGuid)
        {
            var formulas = new List<LibraryCommissionFormulaDetails>();

            var formulaDetails = (from a in db.tblOrganizations
                                  join b in db.lnkCommissionFormulaOrganizations on a.ID equals b.OrgID
                                  join c in db.refCommissionFormulas on b.CommissionFormulaID equals c.ID
                                  join d in db.refCommissionStructures on c.CommissionStructureID equals d.ID
                                  where a.IsActive && a.GUID == orgGuid && b.IsActive && c.IsActive && d.IsActive
                                  select new
                                  {
                                      refCommissionFormula = c,
                                      refCommissionStructure = d
                                  }).ToList();
            var formulaIDs = formulaDetails.Select(a => a.refCommissionFormula.ID).Distinct();
            var rateDetails = (from a in db.lnkCommissionFormulas
                               join b in db.refCommissionRateTypes on a.CommissionRateTypeID equals b.ID
                               where a.IsActive && formulaIDs.Contains(a.CommissionFormulaID) && b.IsActive
                               select new
                               {
                                   FormulaID = a.ID,
                                   refCommissionRateType = b
                               }).ToList();

            // Formula details
            formulas = formulaDetails.Select(a => new LibraryCommissionFormulaDetails()
            {
                CommissionStructureCode = a.refCommissionStructure.Code,
                CommissionStructureName = a.refCommissionStructure.Name,
                FormulaID = a.refCommissionFormula.ID,
                FormulaCode = a.refCommissionFormula.Code,
                FormulaName = a.refCommissionFormula.Name,
            }).ToList();

            // Rate details
            foreach (var formula in formulas)
            {
                var specificRateDetails = rateDetails.Where(a => a.FormulaID == formula.FormulaID).ToList();

                formula.Rates = specificRateDetails.Select(a => new LibraryCommissionRateType()
                {
                    RateCode = a.refCommissionRateType.Code,
                    RateName = a.refCommissionRateType.Name,
                    RequiredValueFrom = a.refCommissionRateType.RequiredValueFrom,
                    RequiredValueTo = a.refCommissionRateType.RequiredValueTo,
                    PayoutAmount = a.refCommissionRateType.PayoutAmount,
                    PayoutPercentage = a.refCommissionRateType.PayoutPercentage,
                }).ToList();
            }

            return formulas;
        }

        public static LibraryCommissionFormulaDetails GetFormula(CommissionIncentiveEntities db, Guid orgGuid, string formulaCode)
        {
            var formula = new LibraryCommissionFormulaDetails();

            var formulaDetail = (from a in db.tblOrganizations
                                 join b in db.lnkCommissionFormulaOrganizations on a.ID equals b.OrgID
                                 join c in db.refCommissionFormulas on b.CommissionFormulaID equals c.ID
                                 join d in db.refCommissionStructures on c.CommissionStructureID equals d.ID
                                 where a.IsActive && a.GUID == orgGuid && b.IsActive && c.IsActive && c.Code == formulaCode && d.IsActive
                                 select new
                                 {
                                     refCommissionFormula = c,
                                     refCommissionStructure = d
                                 }).FirstOrDefault();

            if (formulaDetail == null)
                return formula;

            var formulaID = formulaDetail.refCommissionFormula.ID;
            var rateDetails = (from a in db.lnkCommissionFormulas
                               join b in db.refCommissionRateTypes on a.CommissionRateTypeID equals b.ID
                               where a.IsActive && a.CommissionFormulaID == formulaID && b.IsActive
                               select new
                               {
                                   FormulaID = a.ID,
                                   refCommissionRateType = b
                               }).ToList();

            // Formula details
            formula.CommissionStructureCode = formulaDetail.refCommissionStructure.Code;
            formula.CommissionStructureName = formulaDetail.refCommissionStructure.Name;
            formula.FormulaID = formulaDetail.refCommissionFormula.ID;
            formula.FormulaCode = formulaDetail.refCommissionFormula.Code;
            formula.FormulaName = formulaDetail.refCommissionFormula.Name;

            // Rate details
            formula.Rates = rateDetails.Select(a => new LibraryCommissionRateType()
            {
                RateCode = a.refCommissionRateType.Code,
                RateName = a.refCommissionRateType.Name,
                RequiredValueFrom = a.refCommissionRateType.RequiredValueFrom,
                RequiredValueTo = a.refCommissionRateType.RequiredValueTo,
                PayoutAmount = a.refCommissionRateType.PayoutAmount,
                PayoutPercentage = a.refCommissionRateType.PayoutPercentage,
            }).ToList();

            return formula;
        }

        #endregion

        #region Create / Update

        public static void CreateCommissionBalanceAndJournal(CommissionIncentiveEntities db, CommissionBalanceAndJournalDetails details)
        {
            var balanceBeforeAmount = 0m;

            // tblCommissionBalance
            var tblCommissionBalance = GetCommissionBalance(db, details.OrgID);

            if (tblCommissionBalance != null)
            {
                balanceBeforeAmount = tblCommissionBalance.Balance;

                tblCommissionBalance.Balance = details.IsPlus ? tblCommissionBalance.Balance + details.Amount : tblCommissionBalance.Balance - details.Amount;
                tblCommissionBalance.LastUpdateDT = details.CurrentDT;
                tblCommissionBalance.LastAccessID = details.AccessID;
            }
            else
            {
                tblCommissionBalance = new tblCommissionBalance()
                {
                    OrgID = details.OrgID,
                    Balance = details.Amount,
                    IsActive = true,
                    CreateDT = details.CurrentDT,
                    LastAccessID = details.AccessID
                };
                db.tblCommissionBalances.Add(tblCommissionBalance);
            }
            db.SaveChanges();

            // tblCommissionJournal
            var tblCommissionJournal = new tblCommissionJournal()
            {
                Operation = details.IsPlus ? CommissionJournalOperation.P.ToString() : CommissionJournalOperation.M.ToString(),
                CommissionBalanceID = tblCommissionBalance.ID,
                CommissionID = details.CommissionID,
                PayoutID = details.PayoutID,
                BeforeAmount = balanceBeforeAmount,
                Amount = details.Amount,
                AfterAmount = tblCommissionBalance.Balance,
                IsActive = true,
                CreateDT = details.CurrentDT,
                LastAccessID = details.AccessID
            };
            db.tblCommissionJournals.Add(tblCommissionJournal);
            db.SaveChanges();
        }

        #endregion
    }
}
