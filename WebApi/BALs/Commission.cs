using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Models;
using Data;
using static Library.DBs;
using static WebApi.Helpers.CustomApiResponse;
using static WebApi.Helpers.Log;
using static Library.CommomExtensions;
using Library.Models;

namespace WebApi.BALs
{
    public class CommissionBAL
    {
        private readonly GlobalVariable GLOBE = new GlobalVariable() { VERSION = ApiVersion.One.GetEnumDescription() };

        #region List

        public ApiResponseBody List(CommissionListRequest request)
        {
            try
            {
                var response = new CommissionListResponse();

                return ConstructNotImplementedResponse(response, GLOBE);

                //using (var db = new CommissionIncentiveEntities())
                //{
                //    var error = ListValidation(db, request);
                //    if (!error.IsEmpty())
                //        return ConstructBadRequestResponse(response, GLOBE, error);
                //}

                //return ConstructSuccessResponse(response, GLOBE);
            }
            catch (Exception ex)
            {
                PublicError(ex);
                return ConstructInternalServerErrorResponse(new HealthCheckStatusResponse(), GLOBE);
            }
        }

        private string ListValidation(CommissionIncentiveEntities db, CommissionListRequest request)
        {
            if (request.IsEmpty())
                return ValidationMessage.RequestIsNull.GetEnumDescription();

            if (!IsValidOrg(db, request.OrgRefNo))
                return ValidationMessage.InvalidOrgGuid.GetEnumDescription();

            return "";
        }

        #endregion

        #region Fixed Rate Commission By Amount

        public ApiResponseBody FixedRateCommissionByAmount(FixedRateCommissionByAmountRequest request)
        {
            try
            {
                var response = new FixedRateCommissionByAmountResponse();

                using (var scope = ReadCommittedTransactionScope())
                {
                    using (var db = new CommissionIncentiveEntities())
                    {
                        #region Validation

                        var error = FixedRateCommissionByAmountValidation(db, request);
                        if (!error.IsEmpty())
                            return ConstructBadRequestResponse(response, GLOBE, error);

                        var formula = GetFormula(db, request.OrgRefNo, FormulaCode.F4.ToString());
                        if (formula == null)
                            return ConstructBadRequestResponse(response, GLOBE, ValidationMessage.NoRateConfigure.GetEnumDescription());

                        if (formula.Rates.IsEmpty())
                            return ConstructBadRequestResponse(response, GLOBE, ValidationMessage.NoRateConfigure.GetEnumDescription());

                        #endregion

                        var rate = formula.Rates.OrderByDescending(a => a.RequiredValueFrom).First();
                        var commissionAmount = request.TransactionFigure >= rate.RequiredValueFrom ? rate.PayoutAmount : 0m;

                        var commRefNo = CreateCommissionTransaction(db, request.OrgRefNo, formula.FormulaID, request.TransactionFigure, commissionAmount.ToDecimal());

                        //response
                        response.CommisionAmount = commissionAmount.ToDecimal();
                        response.TransactionDT = GLOBE.DATE_TIME_NOW;
                        response.TransactionFigure = request.TransactionFigure;
                        response.CommissionRefNo = commRefNo;
                    }

                    scope.Complete();
                }

                return ConstructSuccessResponse(response, GLOBE);
            }
            catch (Exception ex)
            {
                PublicError(ex);
                return ConstructInternalServerErrorResponse(new FixedRateCommissionByAmountResponse(), GLOBE);
            }
        }

        private string FixedRateCommissionByAmountValidation(CommissionIncentiveEntities db, FixedRateCommissionByAmountRequest request)
        {
            if (request.IsEmpty())
                return ValidationMessage.RequestIsNull.GetEnumDescription();

            return "";
        }

        #endregion

        #region Fixed Rate Commission By Percentage

        public ApiResponseBody FixedRateCommissionByPercentage(FixedRateCommissionByPercentageRequest request)
        {
            try
            {
                var response = new FixedRateCommissionByPercentageResponse();

                using (var scope = ReadCommittedTransactionScope())
                {
                    using (var db = new CommissionIncentiveEntities())
                    {
                        #region Validation

                        var error = FixedRateCommissionByPercentageValidation(db, request);
                        if (!error.IsEmpty())
                            return ConstructBadRequestResponse(response, GLOBE, error);

                        var formula = GetFormula(db, request.OrgRefNo, FormulaCode.F3.ToString());
                        if (formula == null)
                            return ConstructBadRequestResponse(response, GLOBE, ValidationMessage.NoRateConfigure.GetEnumDescription());

                        if (formula.Rates.IsEmpty())
                            return ConstructBadRequestResponse(response, GLOBE, ValidationMessage.NoRateConfigure.GetEnumDescription());

                        #endregion

                        var rate = formula.Rates.OrderByDescending(a => a.RequiredValueFrom).First();
                        var commissionAmount = request.TransactionFigure >= rate.RequiredValueFrom ? CalculateAmountByPercentage(rate.PayoutPercentage.Value, request.TransactionFigure) : 0m;

                        var commRefNo = CreateCommissionTransaction(db, request.OrgRefNo, formula.FormulaID, request.TransactionFigure, commissionAmount);

                        //response
                        response.CommisionAmount = commissionAmount;
                        response.TransactionDT = GLOBE.DATE_TIME_NOW;
                        response.TransactionFigure = request.TransactionFigure;
                        response.CommissionRefNo = commRefNo;
                    }

                    scope.Complete();
                }

                return ConstructSuccessResponse(response, GLOBE);
            }
            catch (Exception ex)
            {
                PublicError(ex);
                return ConstructInternalServerErrorResponse(new FixedRateCommissionByPercentageResponse(), GLOBE);
            }
        }

        private string FixedRateCommissionByPercentageValidation(CommissionIncentiveEntities db, FixedRateCommissionByPercentageRequest request)
        {
            if (request.IsEmpty())
                return ValidationMessage.RequestIsNull.GetEnumDescription();

            return "";
        }

        #endregion

        #region Tiered Rate Commission By Amount

        public ApiResponseBody TieredRateCommissionByAmount(TieredRateCommissionByAmountRequest request)
        {
            try
            {
                var response = new TieredRateCommissionByAmountResponse();

                using (var scope = ReadCommittedTransactionScope())
                {
                    using (var db = new CommissionIncentiveEntities())
                    {
                        #region Validation

                        var error = FixedRateCommissionByAmountValidation(db, request);
                        if (!error.IsEmpty())
                            return ConstructBadRequestResponse(response, GLOBE, error);

                        var formula = GetFormula(db, request.OrgRefNo, FormulaCode.F1.ToString());
                        if (formula == null)
                            return ConstructBadRequestResponse(response, GLOBE, ValidationMessage.NoRateConfigure.GetEnumDescription());

                        if (formula.Rates.IsEmpty())
                            return ConstructBadRequestResponse(response, GLOBE, ValidationMessage.NoRateConfigure.GetEnumDescription());

                        #endregion

                        var commissionAmount = 0m;
                        var rates = formula.Rates.OrderByDescending(a => a.RequiredValueFrom).ToList();
                        foreach (var rate in rates)
                        {
                            if (request.TransactionFigure >= rate.RequiredValueFrom)
                            {
                                commissionAmount = rate.PayoutAmount ?? 0m;
                                break;
                            }
                        }

                        var commRefNo = CreateCommissionTransaction(db, request.OrgRefNo, formula.FormulaID, request.TransactionFigure, commissionAmount);

                        //response
                        response.CommisionAmount = commissionAmount;
                        response.TransactionDT = GLOBE.DATE_TIME_NOW;
                        response.TransactionFigure = request.TransactionFigure;
                        response.CommissionRefNo = commRefNo;
                    }

                    scope.Complete();
                }

                return ConstructSuccessResponse(response, GLOBE);
            }
            catch (Exception ex)
            {
                PublicError(ex);
                return ConstructInternalServerErrorResponse(new TieredRateCommissionByAmountResponse(), GLOBE);
            }
        }

        private string TieredRateCommissionByAmountValidation(CommissionIncentiveEntities db, TieredRateCommissionByAmountRequest request)
        {
            if (request.IsEmpty())
                return ValidationMessage.RequestIsNull.GetEnumDescription();

            return "";
        }

        #endregion

        #region Tiered Rate Commission By Percentage

        public ApiResponseBody TieredRateCommissionByPercentage(TieredRateCommissionByPercentageRequest request)
        {
            try
            {
                var response = new TieredRateCommissionByPercentageResponse();

                using (var scope = ReadCommittedTransactionScope())
                {
                    using (var db = new CommissionIncentiveEntities())
                    {
                        #region Validation

                        var error = TieredRateCommissionByPercentageValidation(db, request);
                        if (!error.IsEmpty())
                            return ConstructBadRequestResponse(response, GLOBE, error);

                        var formula = GetFormula(db, request.OrgRefNo, FormulaCode.F5.ToString());
                        if (formula == null)
                            return ConstructBadRequestResponse(response, GLOBE, ValidationMessage.NoRateConfigure.GetEnumDescription());

                        if (formula.Rates.IsEmpty())
                            return ConstructBadRequestResponse(response, GLOBE, ValidationMessage.NoRateConfigure.GetEnumDescription());

                        #endregion

                        var commissionAmount = 0m;
                        var rates = formula.Rates.OrderByDescending(a => a.RequiredValueFrom).ToList();
                        foreach (var rate in rates)
                        {
                            if (request.TransactionFigure >= rate.RequiredValueFrom)
                            {
                                commissionAmount = CalculateAmountByPercentage(rate.PayoutPercentage.Value, request.TransactionFigure);
                                break;
                            }
                        }

                        var commRefNo = CreateCommissionTransaction(db, request.OrgRefNo, formula.FormulaID, request.TransactionFigure, commissionAmount);

                        //response
                        response.CommisionAmount = commissionAmount;
                        response.TransactionDT = GLOBE.DATE_TIME_NOW;
                        response.TransactionFigure = request.TransactionFigure;
                        response.CommissionRefNo = commRefNo;
                    }

                    scope.Complete();
                }

                return ConstructSuccessResponse(response, GLOBE);
            }
            catch (Exception ex)
            {
                PublicError(ex);
                return ConstructInternalServerErrorResponse(new TieredRateCommissionByPercentageResponse(), GLOBE);
            }
        }

        private string TieredRateCommissionByPercentageValidation(CommissionIncentiveEntities db, TieredRateCommissionByPercentageRequest request)
        {
            if (request.IsEmpty())
                return ValidationMessage.RequestIsNull.GetEnumDescription();

            return "";
        }

        #endregion

        #region Tiered Rate Commission By Distance Amount

        public ApiResponseBody TieredRateCommissionByDistanceAmount(TieredRateCommissionByDistanceAmountRequest request)
        {
            try
            {
                var response = new TieredRateCommissionByAmountResponse();

                using (var scope = ReadCommittedTransactionScope())
                {
                    using (var db = new CommissionIncentiveEntities())
                    {
                        #region Validation

                        var error = TieredRateCommissionByDistanceAmountValidation(db, request);
                        if (!error.IsEmpty())
                            return ConstructBadRequestResponse(response, GLOBE, error);

                        var formula = GetFormula(db, request.OrgRefNo, FormulaCode.F6.ToString());
                        if (formula == null)
                            return ConstructBadRequestResponse(response, GLOBE, ValidationMessage.NoRateConfigure.GetEnumDescription());

                        if (formula.Rates.IsEmpty())
                            return ConstructBadRequestResponse(response, GLOBE, ValidationMessage.NoRateConfigure.GetEnumDescription());

                        #endregion

                        var commissionAmount = 0m;
                        var rates = formula.Rates.OrderByDescending(a => a.RequiredValueFrom).ToList();
                        foreach (var rate in rates)
                        {
                            if (request.TransactionFigure >= rate.RequiredValueFrom)
                            {
                                commissionAmount = rate.PayoutAmount ?? 0m;
                                break;
                            }
                        }

                        var commRefNo = CreateCommissionTransaction(db, request.OrgRefNo, formula.FormulaID, request.TransactionFigure, commissionAmount);

                        //response
                        response.CommisionAmount = commissionAmount;
                        response.TransactionDT = GLOBE.DATE_TIME_NOW;
                        response.TransactionFigure = request.TransactionFigure;
                        response.CommissionRefNo = commRefNo;
                    }

                    scope.Complete();
                }

                return ConstructSuccessResponse(response, GLOBE);
            }
            catch (Exception ex)
            {
                PublicError(ex);
                return ConstructInternalServerErrorResponse(new TieredRateCommissionByAmountResponse(), GLOBE);
            }
        }

        private string TieredRateCommissionByDistanceAmountValidation(CommissionIncentiveEntities db, TieredRateCommissionByDistanceAmountRequest request)
        {
            if (request.IsEmpty())
                return ValidationMessage.RequestIsNull.GetEnumDescription();

            return "";
        }

        #endregion

        #region Tiered Rate Commission By Distance Percentage

        public ApiResponseBody TieredRateCommissionByDistancePercentage(TieredRateCommissionByDistancePercentageRequest request)
        {
            try
            {
                var response = new TieredRateCommissionByDistancePercentageResponse();

                using (var scope = ReadCommittedTransactionScope())
                {
                    using (var db = new CommissionIncentiveEntities())
                    {
                        #region Validation

                        var error = TieredRateCommissionByDistancePercentageValidation(db, request);
                        if (!error.IsEmpty())
                            return ConstructBadRequestResponse(response, GLOBE, error);

                        var formula = GetFormula(db, request.OrgRefNo, FormulaCode.F5.ToString());
                        if (formula == null)
                            return ConstructBadRequestResponse(response, GLOBE, ValidationMessage.NoRateConfigure.GetEnumDescription());

                        if (formula.Rates.IsEmpty())
                            return ConstructBadRequestResponse(response, GLOBE, ValidationMessage.NoRateConfigure.GetEnumDescription());

                        #endregion

                        var commissionAmount = 0m;
                        var rates = formula.Rates.OrderByDescending(a => a.RequiredValueFrom).ToList();
                        foreach (var rate in rates)
                        {
                            if (request.TransactionFigure >= rate.RequiredValueFrom)
                            {
                                commissionAmount = CalculateAmountByPercentage(rate.PayoutPercentage.Value, request.TransactionFigure);
                                break;
                            }
                        }

                        var commRefNo = CreateCommissionTransaction(db, request.OrgRefNo, formula.FormulaID, request.TransactionFigure, commissionAmount);

                        //response
                        response.CommisionAmount = commissionAmount;
                        response.TransactionDT = GLOBE.DATE_TIME_NOW;
                        response.TransactionFigure = request.TransactionFigure;
                        response.CommissionRefNo = commRefNo;
                    }

                    scope.Complete();
                }

                return ConstructSuccessResponse(response, GLOBE);
            }
            catch (Exception ex)
            {
                PublicError(ex);
                return ConstructInternalServerErrorResponse(new TieredRateCommissionByDistancePercentageResponse(), GLOBE);
            }
        }

        private string TieredRateCommissionByDistancePercentageValidation(CommissionIncentiveEntities db, TieredRateCommissionByDistancePercentageRequest request)
        {
            if (request.IsEmpty())
                return ValidationMessage.RequestIsNull.GetEnumDescription();

            return "";
        }

        #endregion

        #region Payout

        public ApiResponseBody Payout(CommissionPayoutRequest request)
        {
            try
            {
                var response = new CommissionPayoutResponse();

                return ConstructNotImplementedResponse(response, GLOBE);

                //using (var db = new CommissionIncentiveEntities())
                //{
                //    var error = PayoutValidation(db, request);
                //    if (!error.IsEmpty())
                //        return ConstructBadRequestResponse(response, GLOBE, error);
                //}

                //return ConstructSuccessResponse(response, GLOBE);
            }
            catch (Exception ex)
            {
                PublicError(ex);
                return ConstructInternalServerErrorResponse(new HealthCheckStatusResponse(), GLOBE);
            }
        }

        private string PayoutValidation(CommissionIncentiveEntities db, CommissionPayoutRequest request)
        {
            if (request.IsEmpty())
                return ValidationMessage.RequestIsNull.GetEnumDescription();

            if (!IsValidOrg(db, request.OrgRefNo))
                return ValidationMessage.InvalidOrgGuid.GetEnumDescription();

            return "";
        }

        #endregion

        #region Private Functions

        private bool IsValidTransactionFigure(decimal figure)
        {
            return figure >= 1;
        }

        private Guid CreateCommissionTransaction(CommissionIncentiveEntities db, Guid orgRefNo, int formulaID, decimal figure, decimal commission)
        {
            var orgID = GetOrg(db, orgRefNo).ID;

            //trnCommissionTransaction
            var trnCommissionTransaction = new trnCommissionTransaction()
            {
                OrgID = orgID,
                TransactionFigure = figure,
                CommissionFormulaID = formulaID,
                CommissionAmount = commission,
                TransactionDT = GLOBE.DATE_TIME_NOW,
                IsActive = true,
                CreateDT = GLOBE.DATE_TIME_NOW,
                LastAccessID = 0
            };
            db.trnCommissionTransactions.Add(trnCommissionTransaction);
            db.SaveChanges();

            //tblCommissionBalance & tblCommissionJournal
            var commissionBalanceAndJournalDetails = new CommissionBalanceAndJournalDetails()
            {
                CommissionID = trnCommissionTransaction.ID,
                OrgID = orgID,
                IsPlus = true,
                Amount = commission,
                CurrentDT = GLOBE.DATE_TIME_NOW,
                AccessID = 0,
            };
            CreateCommissionBalanceAndJournal(db, commissionBalanceAndJournalDetails);

            return trnCommissionTransaction.GUID;
        }

        private decimal CalculateAmountByPercentage(decimal percentage, decimal amount)
        {
            return (percentage / 100) * amount;
        }

        #endregion
    }
}