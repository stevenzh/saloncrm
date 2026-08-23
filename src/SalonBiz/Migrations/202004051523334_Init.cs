namespace SalonCRM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountRecords",
                c => new
                    {
                        RecordID = c.Long(nullable: false, identity: true),
                        HostID = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        MemberID = c.Long(nullable: false),
                        EventLogID = c.Long(nullable: false),
                        MemberCardId = c.Long(nullable: false),
                        Type = c.String(),
                        SalesType = c.Int(nullable: false),
                        PaymentType = c.String(maxLength: 1),
                        OutAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Debt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FromCardId = c.Long(),
                        BookID = c.Long(),
                        RedeemId = c.Int(),
                        Remark = c.String(maxLength: 200),
                        ClientID = c.String(maxLength: 50),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        IsVaild = c.Int(nullable: false),
                        SaleID = c.String(maxLength: 50),
                        BeauticianID = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.RecordID)
                .ForeignKey("dbo.Organ", t => t.BranchId, cascadeDelete: true)
                .ForeignKey("dbo.Members", t => t.MemberID, cascadeDelete: true)
                .Index(t => t.BranchId)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.Organ",
                c => new
                    {
                        OrganID = c.Int(nullable: false, identity: true),
                        HostID = c.Int(nullable: false),
                        ParentID = c.Int(),
                        Level = c.Int(nullable: false),
                        Name = c.String(maxLength: 50),
                        Manager = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 50),
                        Province = c.String(maxLength: 50),
                        City = c.String(maxLength: 50),
                        Address = c.String(maxLength: 255),
                        IsVaild = c.Int(nullable: false),
                        ClientNum = c.Int(),
                    })
                .PrimaryKey(t => t.OrganID)
                .ForeignKey("dbo.Hosts", t => t.HostID, cascadeDelete: true)
                .Index(t => t.HostID);
            
            CreateTable(
                "dbo.Hosts",
                c => new
                    {
                        HostID = c.Int(nullable: false, identity: true),
                        HostCode = c.String(maxLength: 50),
                        Name = c.String(maxLength: 50),
                        Url = c.String(maxLength: 50),
                        BranchNum = c.Int(),
                        ClientNum = c.Int(),
                        Industry = c.String(maxLength: 50),
                        Province = c.String(maxLength: 50),
                        City = c.String(maxLength: 50),
                        Manager = c.String(maxLength: 50),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Address = c.String(maxLength: 255),
                        IsVaild = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HostID);
            
            CreateTable(
                "dbo.Appointment",
                c => new
                    {
                        AppointmentID = c.Long(nullable: false, identity: true),
                        HostID = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        ClientID = c.String(maxLength: 50),
                        MemberID = c.Long(nullable: false),
                        Name = c.String(maxLength: 50),
                        Phone = c.String(maxLength: 50),
                        BookDate = c.DateTime(nullable: false),
                        Projects = c.String(maxLength: 250),
                        Salesman = c.String(maxLength: 50),
                        Wokers = c.String(maxLength: 250),
                        BookRooms = c.String(maxLength: 250),
                        BookStatus = c.String(maxLength: 50),
                        BookId = c.Long(),
                        CreatedBy = c.String(maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                        Approved = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppointmentID)
                .ForeignKey("dbo.Members", t => t.MemberID, cascadeDelete: true)
                .ForeignKey("dbo.Hosts", t => t.HostID, cascadeDelete: true)
                .Index(t => t.HostID)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        MemberID = c.Long(nullable: false, identity: true),
                        HostID = c.Int(nullable: false),
                        CardNo = c.String(maxLength: 50),
                        JoinDate = c.DateTime(nullable: false),
                        JoinBranch = c.Int(),
                        Source = c.String(maxLength: 50),
                        Passwd = c.String(maxLength: 50),
                        Type = c.String(maxLength: 50),
                        Level = c.String(maxLength: 50),
                        Status = c.String(maxLength: 50),
                        BookTime = c.Int(nullable: false),
                        Amt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Points = c.Int(nullable: false),
                        IsNew = c.Int(nullable: false),
                        Remark = c.String(maxLength: 2000),
                        CreatedBy = c.String(maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        FeedbackDate = c.DateTime(),
                        Feedback = c.String(maxLength: 50),
                        LastBirth = c.DateTime(),
                        SalesmanId = c.String(maxLength: 200),
                        BeauticianId = c.String(maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        MobileNumber = c.String(maxLength: 50),
                        WebChat = c.String(maxLength: 50),
                        TencentQQ = c.String(maxLength: 50),
                        Email = c.String(maxLength: 50),
                        Sex = c.String(maxLength: 1),
                        Address = c.String(maxLength: 250),
                        CompanyAddress = c.String(maxLength: 250),
                        WeddingDay = c.DateTime(),
                        Birthday = c.DateTime(),
                        Vocation = c.String(maxLength: 50),
                        Position = c.String(maxLength: 50),
                        Company = c.String(maxLength: 50),
                        MaritalStatus = c.String(maxLength: 50),
                        Conjugal = c.String(maxLength: 200),
                        SkinType = c.String(maxLength: 50),
                        SkinConditions = c.String(maxLength: 100),
                        FacialDemand = c.String(maxLength: 100),
                        BodyDemand = c.String(maxLength: 100),
                        CustomerDemand = c.String(maxLength: 100),
                        ConsumptionHabit = c.String(maxLength: 100),
                        Personality = c.String(maxLength: 100),
                        OpenID = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.MemberID)
                .ForeignKey("dbo.ApplicationUser", t => t.SalesmanId)
                .Index(t => t.SalesmanId);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookID = c.Long(nullable: false, identity: true),
                        HostID = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        MemberID = c.Long(nullable: false),
                        LogId = c.Long(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Satisfaction = c.Int(),
                        Remark = c.String(maxLength: 2000),
                        ClientID = c.String(maxLength: 50),
                        CreatedBy = c.String(maxLength: 200),
                        CreatedDate = c.DateTime(nullable: false),
                        SalesmanID = c.String(maxLength: 200),
                        State = c.String(maxLength: 20),
                        PaymentID = c.String(maxLength: 50),
                        PayTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.BookID)
                .ForeignKey("dbo.Members", t => t.MemberID, cascadeDelete: true)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.BookProjects",
                c => new
                    {
                        BookProjectID = c.Long(nullable: false, identity: true),
                        BookID = c.Long(nullable: false),
                        MemberCardId = c.Long(),
                        MemberProjectId = c.Long(),
                        MemberGiveId = c.Long(),
                        ProjectID = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Points = c.Int(),
                        HandicraftFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Satisfaction = c.Int(),
                        Appraisal = c.String(maxLength: 500),
                        BeauticianId = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.BookProjectID)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.BookID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.BookGoods",
                c => new
                    {
                        BookGoodsID = c.Long(nullable: false, identity: true),
                        BookProjectID = c.Long(nullable: false),
                        BookID = c.Long(nullable: false),
                        GoodsID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.BookGoodsID)
                .ForeignKey("dbo.BookProjects", t => t.BookProjectID, cascadeDelete: true)
                .ForeignKey("dbo.Goods", t => t.GoodsID, cascadeDelete: true)
                .Index(t => t.BookProjectID)
                .Index(t => t.GoodsID);
            
            CreateTable(
                "dbo.Goods",
                c => new
                    {
                        GoodsID = c.Int(nullable: false, identity: true),
                        HostID = c.Int(nullable: false),
                        GoodsCode = c.String(maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        Unit = c.String(maxLength: 50),
                        Category = c.String(maxLength: 50),
                        Brand = c.String(maxLength: 50),
                        IsVaild = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GoodsID);
            
            CreateTable(
                "dbo.MemberProjectGoods",
                c => new
                    {
                        MemberProjectGoodsID = c.Long(nullable: false, identity: true),
                        MemberProjectId = c.Long(nullable: false),
                        GoodsID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.MemberProjectGoodsID)
                .ForeignKey("dbo.Goods", t => t.GoodsID, cascadeDelete: true)
                .ForeignKey("dbo.MemberProjects", t => t.MemberProjectId, cascadeDelete: true)
                .Index(t => t.MemberProjectId)
                .Index(t => t.GoodsID);
            
            CreateTable(
                "dbo.MemberProjects",
                c => new
                    {
                        MemberProjectId = c.Long(nullable: false, identity: true),
                        HostID = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        MemberID = c.Long(nullable: false),
                        LogId = c.Long(nullable: false),
                        AccountRecordID = c.Long(),
                        MemberCardId = c.Long(),
                        ProjectID = c.Int(nullable: false),
                        Type = c.String(maxLength: 20),
                        DebtFlag = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BookTime = c.Int(nullable: false),
                        UsedTime = c.Int(nullable: false),
                        LastCount = c.Int(nullable: false),
                        IsEntity = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        ClientId = c.String(maxLength: 50),
                        CreatedBy = c.String(maxLength: 50),
                        ExpiryDate = c.DateTime(),
                        IsVaild = c.Int(nullable: false),
                        status = c.Int(nullable: false),
                        Remark = c.String(maxLength: 2000),
                        GiveId = c.Long(),
                    })
                .PrimaryKey(t => t.MemberProjectId)
                .ForeignKey("dbo.Members", t => t.MemberID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.MemberID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectID = c.Int(nullable: false, identity: true),
                        HostID = c.Int(nullable: false),
                        Code = c.String(maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        MinUnit = c.Int(nullable: false),
                        HandicraftFee = c.Int(nullable: false),
                        LowHandicraftFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Brand = c.String(maxLength: 50),
                        Category = c.String(nullable: false, maxLength: 50),
                        ExtCategory = c.String(maxLength: 50),
                        SecCategory = c.Int(nullable: false),
                        IsEntity = c.Int(nullable: false),
                        Status = c.String(maxLength: 50),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectID)
                .ForeignKey("dbo.Hosts", t => t.HostID, cascadeDelete: true)
                .Index(t => t.HostID);
            
            CreateTable(
                "dbo.MemberGives",
                c => new
                    {
                        GiveId = c.Long(nullable: false, identity: true),
                        HostID = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        MemberID = c.Long(nullable: false),
                        LogId = c.Long(nullable: false),
                        ProjectID = c.Int(),
                        Type = c.String(maxLength: 20),
                        InPoints = c.Int(nullable: false),
                        RemainPoints = c.Int(nullable: false),
                        BookTime = c.Int(nullable: false),
                        UsedTime = c.Int(nullable: false),
                        LastCount = c.Int(nullable: false),
                        Salesman = c.String(maxLength: 50),
                        CreateDate = c.DateTime(nullable: false),
                        ClientId = c.String(maxLength: 50),
                        CreatedBy = c.String(maxLength: 50),
                        ExpiryDate = c.DateTime(),
                        Remark = c.String(maxLength: 500),
                        IsVaild = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GiveId)
                .ForeignKey("dbo.Members", t => t.MemberID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID)
                .Index(t => t.MemberID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.ProjectGoods",
                c => new
                    {
                        ProjectGoodsID = c.Int(nullable: false, identity: true),
                        GoodsID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.ProjectGoodsID)
                .ForeignKey("dbo.Goods", t => t.GoodsID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.GoodsID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.CardTmplProject",
                c => new
                    {
                        TmplProjectID = c.Int(nullable: false, identity: true),
                        TmplID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(),
                        Amount = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.TmplProjectID)
                .ForeignKey("dbo.CardTmpl", t => t.TmplID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.TmplID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.CardTmpl",
                c => new
                    {
                        TmplID = c.Int(nullable: false, identity: true),
                        HostID = c.Int(nullable: false),
                        CardType = c.String(nullable: false, maxLength: 20),
                        Title = c.String(maxLength: 50),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsVaild = c.Int(nullable: false),
                        Remark = c.String(maxLength: 500),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TmplID);
            
            CreateTable(
                "dbo.BookProjectSplits",
                c => new
                    {
                        SplitID = c.Long(nullable: false, identity: true),
                        BookProjectID = c.Long(nullable: false),
                        Position = c.String(maxLength: 10),
                        UserID = c.String(nullable: false, maxLength: 200),
                        Percentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HandicraftFee = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(maxLength: 200),
                        ModifiedTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SplitID)
                .ForeignKey("dbo.ApplicationUser", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.BookProjects", t => t.BookProjectID, cascadeDelete: true)
                .Index(t => t.BookProjectID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.ApplicationUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 200),
                        HostId = c.Int(nullable: false),
                        OrganId = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(maxLength: 500),
                        Password = c.String(nullable: false, maxLength: 50),
                        Type = c.String(maxLength: 500),
                        IsAdminUser = c.Int(nullable: false),
                        UserCnName = c.String(maxLength: 500),
                        Rank = c.String(maxLength: 50),
                        JoinDate = c.DateTime(),
                        ResignDate = c.DateTime(),
                        Position = c.String(maxLength: 20),
                        MobileNumber = c.String(maxLength: 200),
                        IsMajorOrgan = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        Status = c.String(maxLength: 200),
                        OpenID = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AccountRecordSplits",
                c => new
                    {
                        SplitID = c.Long(nullable: false, identity: true),
                        RecordID = c.Long(nullable: false),
                        Position = c.String(maxLength: 10),
                        UserID = c.String(nullable: false, maxLength: 200),
                        Percentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Remark = c.String(maxLength: 200),
                        ModifiedTime = c.DateTime(nullable: false),
                        ModifiedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.SplitID)
                .ForeignKey("dbo.ApplicationUser", t => t.UserID, cascadeDelete: true)
                .ForeignKey("dbo.AccountRecords", t => t.RecordID, cascadeDelete: true)
                .Index(t => t.RecordID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.ApplicationRole",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 200),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(maxLength: 500),
                        IsMajor = c.Boolean(nullable: false),
                        HostID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 500),
                        MenuPath = c.String(maxLength: 200),
                        Icon = c.String(maxLength: 20),
                        Level = c.Int(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        SiteNav = c.String(maxLength: 20),
                        SiteNavNext = c.String(maxLength: 20),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Feedback",
                c => new
                    {
                        FeedbackId = c.Long(nullable: false, identity: true),
                        HostId = c.Int(nullable: false),
                        MemberId = c.Long(nullable: false),
                        Purpose = c.String(maxLength: 50),
                        LinkWay = c.String(maxLength: 50),
                        Result = c.String(maxLength: 50),
                        NextDate = c.DateTime(),
                        CreatedDate = c.DateTime(nullable: false),
                        CallUserId = c.String(nullable: false, maxLength: 50),
                        Centent = c.String(maxLength: 2000),
                        BranchId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.FeedbackId)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.MemberCards",
                c => new
                    {
                        MemberCardId = c.Long(nullable: false, identity: true),
                        MemberID = c.Long(nullable: false),
                        LogId = c.Long(nullable: false),
                        TmplID = c.Int(),
                        Type = c.String(maxLength: 1),
                        Title = c.String(maxLength: 200),
                        Amt = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ExpiryDate = c.DateTime(),
                        BookTime = c.Int(nullable: false),
                        UsedTime = c.Int(nullable: false),
                        LastCount = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DebtFlag = c.Int(nullable: false),
                        DebtStatus = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        CreateDate = c.DateTime(nullable: false),
                        HostID = c.Int(nullable: false),
                        BranchID = c.Int(nullable: false),
                        ClientID = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.MemberCardId)
                .ForeignKey("dbo.Members", t => t.MemberID, cascadeDelete: true)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.MemberCardProjects",
                c => new
                    {
                        MemberCardProjectId = c.Long(nullable: false, identity: true),
                        MemberCardId = c.Long(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.MemberCardProjectId)
                .ForeignKey("dbo.MemberCards", t => t.MemberCardId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .Index(t => t.MemberCardId)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.PointBooks",
                c => new
                    {
                        PointBookId = c.Long(nullable: false, identity: true),
                        HostId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        MemberId = c.Long(nullable: false),
                        LogId = c.Long(nullable: false),
                        OutPoints = c.Int(nullable: false),
                        InPoints = c.Int(nullable: false),
                        RemainPoints = c.Int(nullable: false),
                        ClientId = c.String(),
                        InOut = c.Int(nullable: false),
                        MemberCardId = c.Long(),
                        ExpiryDate = c.DateTime(),
                        Salesman = c.String(),
                        Remark = c.String(),
                        CreatedBy = c.String(),
                        CreatedDate = c.DateTime(nullable: false),
                        GiveId = c.Long(),
                    })
                .PrimaryKey(t => t.PointBookId)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.RedeemProject",
                c => new
                    {
                        RedeemId = c.Int(nullable: false, identity: true),
                        HostId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        MemberId = c.Long(nullable: false),
                        MemberProjectId = c.Long(nullable: false),
                        LogId = c.Long(nullable: false),
                        ClientId = c.String(maxLength: 50),
                        ProjectId = c.Int(nullable: false),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Count = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        Remark = c.String(maxLength: 500),
                        CardLogId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.RedeemId)
                .ForeignKey("dbo.Members", t => t.MemberId, cascadeDelete: true)
                .Index(t => t.MemberId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        ClientID = c.Int(nullable: false, identity: true),
                        HostID = c.Int(nullable: false),
                        OrganID = c.Int(),
                        MobileGUID = c.String(maxLength: 50),
                        MobileNumber = c.String(maxLength: 50),
                        MobileModel = c.String(maxLength: 200),
                        IsVaild = c.String(maxLength: 20),
                        UserId = c.String(maxLength: 200),
                        LastSignIn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ClientID)
                .ForeignKey("dbo.Hosts", t => t.HostID, cascadeDelete: true)
                .Index(t => t.HostID);
            
            CreateTable(
                "dbo.Dictionary",
                c => new
                    {
                        TypeId = c.Long(nullable: false, identity: true),
                        HostId = c.Int(nullable: false),
                        Identifier = c.String(nullable: false, maxLength: 50),
                        KeyValue = c.String(nullable: false, maxLength: 50),
                        Contents = c.String(nullable: false, maxLength: 50),
                        Shell = c.String(maxLength: 500),
                        Remark = c.String(maxLength: 500),
                        IsVaild = c.Int(nullable: false),
                        SortOrder = c.Int(nullable: false),
                        IsDefault = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.TypeId);
            
            CreateTable(
                "dbo.EventLog",
                c => new
                    {
                        LogId = c.Long(nullable: false, identity: true),
                        HostId = c.Int(nullable: false),
                        BranchId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(maxLength: 50),
                        MemberId = c.Long(),
                        TypeId = c.Int(nullable: false),
                        Level = c.Int(nullable: false),
                        Content = c.String(),
                        Shell = c.String(maxLength: 2000),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.LogId);
            
            CreateTable(
                "dbo.HostProfiles",
                c => new
                    {
                        ProfileID = c.Int(nullable: false, identity: true),
                        HostID = c.Int(nullable: false),
                        PropertyValue = c.String(maxLength: 255),
                        PropertyText = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ProfileID);
            
            CreateTable(
                "dbo.Objective",
                c => new
                    {
                        ObjectiveId = c.Long(nullable: false, identity: true),
                        Level = c.Int(nullable: false),
                        OrganId = c.Int(nullable: false),
                        TeamId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 256),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Accounts = c.Int(nullable: false),
                        TopObjective = c.Int(nullable: false),
                        SalesObjective = c.Int(nullable: false),
                        ServiceObjective = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ObjectiveId);
            
            CreateTable(
                "dbo.ProjectCategorys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(),
                        Name = c.String(nullable: false, maxLength: 200),
                        Description = c.String(maxLength: 500),
                        Level = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        Code = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false, maxLength: 255),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Code);
            
            CreateTable(
                "dbo.WxMembers",
                c => new
                    {
                        MemberID = c.Long(nullable: false, identity: true),
                        WxMemberID = c.Long(nullable: false),
                        HostID = c.Int(nullable: false),
                        LastMessageTime = c.DateTime(),
                        Binding = c.String(maxLength: 5),
                        EmployeeID = c.String(maxLength: 200),
                        OpenID = c.String(nullable: false, maxLength: 50),
                        NickName = c.String(maxLength: 500),
                        Sex = c.Int(),
                        Language = c.String(maxLength: 20),
                        City = c.String(maxLength: 50),
                        Province = c.String(maxLength: 50),
                        Country = c.String(maxLength: 50),
                        HeadImgUrl = c.String(maxLength: 500),
                        SubscribeTime = c.DateTime(nullable: false),
                        Subscribe = c.String(maxLength: 1),
                        UnsubscribeTime = c.DateTime(),
                    })
                .PrimaryKey(t => t.MemberID);
            
            CreateTable(
                "dbo.RoleMenus",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 200),
                        MenuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.RoleId, t.MenuId })
                .ForeignKey("dbo.ApplicationRole", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.MenuItems", t => t.MenuId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.MenuId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 200),
                        RoleId = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.ApplicationUser", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.ApplicationRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AccountRecordSplits", "RecordID", "dbo.AccountRecords");
            DropForeignKey("dbo.AccountRecords", "MemberID", "dbo.Members");
            DropForeignKey("dbo.AccountRecords", "BranchId", "dbo.Organ");
            DropForeignKey("dbo.Organ", "HostID", "dbo.Hosts");
            DropForeignKey("dbo.Clients", "HostID", "dbo.Hosts");
            DropForeignKey("dbo.Appointment", "HostID", "dbo.Hosts");
            DropForeignKey("dbo.Appointment", "MemberID", "dbo.Members");
            DropForeignKey("dbo.Members", "SalesmanId", "dbo.ApplicationUser");
            DropForeignKey("dbo.RedeemProject", "MemberId", "dbo.Members");
            DropForeignKey("dbo.PointBooks", "MemberId", "dbo.Members");
            DropForeignKey("dbo.MemberCardProjects", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.MemberCardProjects", "MemberCardId", "dbo.MemberCards");
            DropForeignKey("dbo.MemberCards", "MemberID", "dbo.Members");
            DropForeignKey("dbo.Feedback", "MemberId", "dbo.Members");
            DropForeignKey("dbo.Books", "MemberID", "dbo.Members");
            DropForeignKey("dbo.BookProjectSplits", "BookProjectID", "dbo.BookProjects");
            DropForeignKey("dbo.BookProjectSplits", "UserID", "dbo.ApplicationUser");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.ApplicationRole");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.ApplicationUser");
            DropForeignKey("dbo.RoleMenus", "MenuId", "dbo.MenuItems");
            DropForeignKey("dbo.RoleMenus", "RoleId", "dbo.ApplicationRole");
            DropForeignKey("dbo.AccountRecordSplits", "UserID", "dbo.ApplicationUser");
            DropForeignKey("dbo.BookProjects", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.BookGoods", "GoodsID", "dbo.Goods");
            DropForeignKey("dbo.MemberProjectGoods", "MemberProjectId", "dbo.MemberProjects");
            DropForeignKey("dbo.MemberProjects", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.CardTmplProject", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.CardTmplProject", "TmplID", "dbo.CardTmpl");
            DropForeignKey("dbo.ProjectGoods", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.ProjectGoods", "GoodsID", "dbo.Goods");
            DropForeignKey("dbo.MemberGives", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.MemberGives", "MemberID", "dbo.Members");
            DropForeignKey("dbo.Projects", "HostID", "dbo.Hosts");
            DropForeignKey("dbo.MemberProjects", "MemberID", "dbo.Members");
            DropForeignKey("dbo.MemberProjectGoods", "GoodsID", "dbo.Goods");
            DropForeignKey("dbo.BookGoods", "BookProjectID", "dbo.BookProjects");
            DropForeignKey("dbo.BookProjects", "BookID", "dbo.Books");
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.RoleMenus", new[] { "MenuId" });
            DropIndex("dbo.RoleMenus", new[] { "RoleId" });
            DropIndex("dbo.Clients", new[] { "HostID" });
            DropIndex("dbo.RedeemProject", new[] { "MemberId" });
            DropIndex("dbo.PointBooks", new[] { "MemberId" });
            DropIndex("dbo.MemberCardProjects", new[] { "ProjectID" });
            DropIndex("dbo.MemberCardProjects", new[] { "MemberCardId" });
            DropIndex("dbo.MemberCards", new[] { "MemberID" });
            DropIndex("dbo.Feedback", new[] { "MemberId" });
            DropIndex("dbo.AccountRecordSplits", new[] { "UserID" });
            DropIndex("dbo.AccountRecordSplits", new[] { "RecordID" });
            DropIndex("dbo.BookProjectSplits", new[] { "UserID" });
            DropIndex("dbo.BookProjectSplits", new[] { "BookProjectID" });
            DropIndex("dbo.CardTmplProject", new[] { "ProjectID" });
            DropIndex("dbo.CardTmplProject", new[] { "TmplID" });
            DropIndex("dbo.ProjectGoods", new[] { "ProjectID" });
            DropIndex("dbo.ProjectGoods", new[] { "GoodsID" });
            DropIndex("dbo.MemberGives", new[] { "ProjectID" });
            DropIndex("dbo.MemberGives", new[] { "MemberID" });
            DropIndex("dbo.Projects", new[] { "HostID" });
            DropIndex("dbo.MemberProjects", new[] { "ProjectID" });
            DropIndex("dbo.MemberProjects", new[] { "MemberID" });
            DropIndex("dbo.MemberProjectGoods", new[] { "GoodsID" });
            DropIndex("dbo.MemberProjectGoods", new[] { "MemberProjectId" });
            DropIndex("dbo.BookGoods", new[] { "GoodsID" });
            DropIndex("dbo.BookGoods", new[] { "BookProjectID" });
            DropIndex("dbo.BookProjects", new[] { "ProjectID" });
            DropIndex("dbo.BookProjects", new[] { "BookID" });
            DropIndex("dbo.Books", new[] { "MemberID" });
            DropIndex("dbo.Members", new[] { "SalesmanId" });
            DropIndex("dbo.Appointment", new[] { "MemberID" });
            DropIndex("dbo.Appointment", new[] { "HostID" });
            DropIndex("dbo.Organ", new[] { "HostID" });
            DropIndex("dbo.AccountRecords", new[] { "MemberID" });
            DropIndex("dbo.AccountRecords", new[] { "BranchId" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.RoleMenus");
            DropTable("dbo.WxMembers");
            DropTable("dbo.Region");
            DropTable("dbo.ProjectCategorys");
            DropTable("dbo.Objective");
            DropTable("dbo.HostProfiles");
            DropTable("dbo.EventLog");
            DropTable("dbo.Dictionary");
            DropTable("dbo.Clients");
            DropTable("dbo.RedeemProject");
            DropTable("dbo.PointBooks");
            DropTable("dbo.MemberCardProjects");
            DropTable("dbo.MemberCards");
            DropTable("dbo.Feedback");
            DropTable("dbo.MenuItems");
            DropTable("dbo.ApplicationRole");
            DropTable("dbo.AccountRecordSplits");
            DropTable("dbo.ApplicationUser");
            DropTable("dbo.BookProjectSplits");
            DropTable("dbo.CardTmpl");
            DropTable("dbo.CardTmplProject");
            DropTable("dbo.ProjectGoods");
            DropTable("dbo.MemberGives");
            DropTable("dbo.Projects");
            DropTable("dbo.MemberProjects");
            DropTable("dbo.MemberProjectGoods");
            DropTable("dbo.Goods");
            DropTable("dbo.BookGoods");
            DropTable("dbo.BookProjects");
            DropTable("dbo.Books");
            DropTable("dbo.Members");
            DropTable("dbo.Appointment");
            DropTable("dbo.Hosts");
            DropTable("dbo.Organ");
            DropTable("dbo.AccountRecords");
        }
    }
}
