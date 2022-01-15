using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationManagementSystem.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationManagementSystem.Entities.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);
            builder.HasDiscriminator<string>("NotificationType").HasValue<ApplicationNotification>("Viewed");
            builder.HasOne(n => n.Sender).WithMany();
            builder.HasMany(n => n.Receivers).WithMany(e => e.Notifications);
            builder.Property(n => n.Title).IsRequired();
            builder.Property(n => n.Message).IsRequired();
        }
    }
}
