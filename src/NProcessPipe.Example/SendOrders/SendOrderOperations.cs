//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace NProcessPipe.Example.SendOrders
//{

//    public class PrintDeliverySticker : IOperation<SendOrderRow>
//    {
//    }

//    public class PrintInvoice : IOperation<SendOrderRow>
//    {
//    }

    
//    public class CreatePickingSlip : IOperation<SendOrderRow>
//    {
//    }

//    public class PrintPickingSlip : IOperation<SendOrderRow>, IOperationDependsOn<CreatePickingSlip>
//    {
//    }


//    public class GatherPaperwork : IOperation<SendOrderRow>,
//        IOperationDependsOn<PrintDeliverySticker>,
//        IOperationDependsOn<PrintInvoice>,
//        IOperationDependsOn<PrintPickingSlip>
//    {
//    }
//}
