using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

using System.Linq;
using System.Collections.Generic;
using CoreGraphics;

namespace LycheeClientPortal.iOS
{
    public class ProjectInvoiceSource : UITableViewSource
    {
        string CellIdentifier = "projectInvoiceTable";
        UIViewController parentController;
        List<Invoice> TableItems = new List<Invoice>();
        bool hideHistroySection;
        int numLateInvoice;

        public ProjectInvoiceSource(UIViewController controllerr, List<Invoice> items)
        {
            parentController = controllerr;
            TableItems = items.OrderByDescending(x => x.InvoiceDate).ToList();
            hideHistroySection = true;
            numLateInvoice = TableItems.Where(x => x.InvoiceStateValue == InvoiceStateValue.late).Count();
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier);


            if (indexPath.Section == 2)
            {
                cell = new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
                if (hideHistroySection)
                {
                    cell.TextLabel.Text = string.Format("Show older invoices ({0})", TableItems.Count - 3);
                    cell.TextLabel.TextColor = UIColor.Green;
                }
                else
                {
                    cell.TextLabel.Text = string.Format("Hide older invoices");
                    cell.TextLabel.TextColor = UIColor.Red;
                }
                return cell;
            }


            Invoice item = TableItems[indexPath.Row];
            if (indexPath.Section == 1 && TableItems.Count > 3)
            {
                item = TableItems[indexPath.Row + 3];
            }


            cell = new UITableViewCell(UITableViewCellStyle.Subtitle, CellIdentifier);
            cell.TextLabel.Text = "$" + item.InvoiceAmount.ToString();
            string info;
            if (item.InvoiceStateValue == InvoiceStateValue.open || item.InvoiceStateValue == InvoiceStateValue.late)
            {
                info = "Invoked On " + item.InvoiceDate.ToString("dd-MMM-yyyy");
                cell.TextLabel.Text += " Due";
            }
            else
            {
                info = "Paid On " + ((DateTime)item.PaidDate).ToString("dd-MMM-yyyy") + " - Thank you";
            }
            cell.DetailTextLabel.Text = info;
            cell.Accessory = UITableViewCellAccessory.None;

            string imgsource = "";
            if (item.InvoiceStateValue == InvoiceStateValue.open)
            {
                imgsource += "INVOICE_OPEN.png";
            }
            else if (item.InvoiceStateValue == InvoiceStateValue.paid)
            {
                imgsource += "INVOICE_PAID.png";
            }
            else if (item.InvoiceStateValue == InvoiceStateValue.late)
            {
                imgsource += "INVOICE_LATE.png";
            }

            UIImageView imageView;
            imageView = new UIImageView
            {
                Image = UIImage.FromBundle(imgsource),
                Frame = new CGRect(cell.Bounds.Width - cell.Bounds.Height, 0, cell.Bounds.Height, cell.Bounds.Height)
            };
            cell.AddSubview(imageView);

            return cell;
        }




        public override nint NumberOfSections(UITableView tableView)
        {
            return 3;
        }
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (section == 0)
            {
                if (TableItems.Count > 3)
                    return 3;
                else
                    return TableItems.Count;
            }
            else if (section == 1)
            {
                if (TableItems.Count > 3 && !hideHistroySection)
                    return TableItems.Count - 3;
                else
                    return 0;
            }
            else//section == 2
            {
                if (TableItems.Count > 3)
                    return 1;
                else
                    return 0;
            }
        }


        public override string TitleForHeader(UITableView tableView, nint section)
        {
            string sectionTitle = "";

            if (section == 0)
                sectionTitle = "Recent Invoices";
            else if (section == 1 && !hideHistroySection)
                sectionTitle = "Older Invoices";

            return sectionTitle;
        }
        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (section == 1 && hideHistroySection)
                return 1;
            return 30;
        }
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Section == 2)
            {
                hideHistroySection = !hideHistroySection;
                tableView.ReloadData();
            }
            if (TableItems[indexPath.Row].InvoiceStateValue == InvoiceStateValue.late)
            {
                //Create Alert
                var okCancelAlertController = UIAlertController.Create("Late Invoice", "Are you sure you want to dismiss this reminder?", UIAlertControllerStyle.Alert);

                //Add Actions
                okCancelAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, (action) => {
                    numLateInvoice--;
                    if (numLateInvoice <= 0)
                        ((UITabBarController)parentController).TabBar.Items[2].BadgeValue = null;
                    else
                    {
                        if (numLateInvoice > 9)
                            ((UITabBarController)parentController).TabBar.Items[2].BadgeValue = "9+";
                        else
                            ((UITabBarController)parentController).TabBar.Items[2].BadgeValue = numLateInvoice.ToString();
                    }
                }));
                okCancelAlertController.AddAction(UIAlertAction.Create("Cancel", UIAlertActionStyle.Cancel, (alert) => { }));

                //Present Alert
                parentController.PresentViewController(okCancelAlertController, true, null);
            }
            tableView.DeselectRow(indexPath, true);
        }
    }

    partial class ProjectDetailInvoiceController : UIViewController
	{
        UITableView projectInvoiceTable;

        public List<Invoice> InvoiceList { get; set; }
        public ProjectDetailInvoiceController(IntPtr handle) : base(handle)
        {
            projectInvoiceTable = new UITableView(new CGRect(0, 0, 0, 0), UITableViewStyle.Grouped);

            InvoiceList = ((ProjectDetailController)ParentViewController).thisProject.invoices;
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //update view after loading
            initProjectInvoiceTable();
        }
        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            projectInvoiceTable.Frame = new CGRect(
                0,
                ((ProjectDetailController)ParentViewController).titleViewYOffSet,
                UIScreen.MainScreen.Bounds.Width,
                UIScreen.MainScreen.Bounds.Height - ((ProjectDetailController)ParentViewController).titleViewYOffSet - ((UITabBarController)ParentViewController).TabBar.Bounds.Height);
            View.AddSubview(projectInvoiceTable);
        }

        void initProjectInvoiceTable()
        {
            projectInvoiceTable.SectionFooterHeight = 0;
            projectInvoiceTable.Source = new ProjectInvoiceSource(ParentViewController, InvoiceList);
        }
    }
}
