using System.ComponentModel.DataAnnotations;

namespace WSM.Domain.Entities
{

    /// <summary>
    /// Base class representing a user-related entity with common properties such as
    /// a unique identifier, role information, and audit fields for creation and modification tracking.
    /// </summary>
    /// <typeparam name="T">The type of the unique identifier for the entity (e.g., ULID, GUID).</typeparam>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// This identifier is typically a ULID, ensuring both uniqueness and
        /// lexicographical sorting, which is useful for ordered storage and retrieval.
        /// </summary>
        /// <example>01F8MECHZX3TBDSZ7FHT9V0J2K</example>
        [Required]
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the timestamp for when the entity was created.
        /// This field is used for auditing and helps in tracking the age of the record.
        /// </summary>
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the timestamp for the last modification of the entity.
        /// Used for tracking the most recent update to this record.
        /// </summary>
        public DateTime? ModifiedAt { get; set; } = DateTime.UtcNow;
    }
}