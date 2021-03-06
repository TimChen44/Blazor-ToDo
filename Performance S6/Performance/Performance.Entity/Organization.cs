﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Performance.Entity
{
    [Table("Organization")]
    public partial class Organization
    {
        public Organization()
        {
            Achievements = new HashSet<Achievement>();
            InverseParent = new HashSet<Organization>();
            OrganizationIndices = new HashSet<OrganizationIndex>();
        }

        /// <summary>
        /// 机构/部门
        /// </summary>
        [Key]
        public Guid OrganizationId { get; set; }
        /// <summary>
        /// 父ID
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [StringLength(100)]
        public string Address { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        [StringLength(100)]
        public string Phone { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(100)]
        public string Description { get; set; }

        [ForeignKey(nameof(ParentId))]
        [InverseProperty(nameof(Organization.InverseParent))]
        public virtual Organization Parent { get; set; }
        [InverseProperty(nameof(Achievement.Organization))]
        public virtual ICollection<Achievement> Achievements { get; set; }
        [InverseProperty(nameof(Organization.Parent))]
        public virtual ICollection<Organization> InverseParent { get; set; }
        [InverseProperty(nameof(OrganizationIndex.Organization))]
        public virtual ICollection<OrganizationIndex> OrganizationIndices { get; set; }
    }
}