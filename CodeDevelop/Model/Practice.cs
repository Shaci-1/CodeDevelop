//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CodeDevelop.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Practice
    {
        public int Id { get; set; }
        public string text { get; set; }
        public int trainingId { get; set; }
        public string name { get; set; }
        public byte[] screenShot { get; set; }
    
        public virtual Training Training { get; set; }
    }
}
