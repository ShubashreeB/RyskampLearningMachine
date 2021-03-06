﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RNN.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace RNN.Models
{
    public class Input
    {       
 
        public Int64 ID { get; set; }
        public String Name { get; set; }
        public double Max { get; set; }
        public double Min { get; set; }
        public RnnInputType Type { get; set; }
        public long Rnetwork_ID { get; set; }
        public long Input_Output_Type_ID { get; set; }

        //Navigation Proprties
        [ForeignKey("Rnetwork_ID")]
        public virtual Rnetwork Rnetwork { get; set; }
        [ForeignKey("Input_Output_Type_ID")]
        public virtual Input_Output_Type Input_Output_Type { get; set; }
        public virtual ICollection<Input_Values_Rneuron> Input_Values_Reneurons { get; set; }

        //Constructors
        //One parameterless constructor is required by EF for auto creation
        public Input()
        {
            Input_Values_Reneurons = new HashSet<Input_Values_Rneuron>();
        }

    }
}
