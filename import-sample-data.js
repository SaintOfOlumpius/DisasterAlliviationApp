// MongoDB import script for sample data
// Run this with: mongosh DisasterAlleviationDB import-sample-data.js

use('DisasterAlleviationDB');

// Clear existing collections (optional - remove if you want to keep existing data)
db.Donations.deleteMany({});
db.Beneficiaries.deleteMany({});
db.Volunteers.deleteMany({});
db.Disasters.deleteMany({});

// Insert Donations
db.Donations.insertMany([
  {
    Type: "Money",
    Description: "Cash donation for disaster relief",
    Amount: 5000.00,
    Date: new Date("2024-01-15T10:00:00Z")
  },
  {
    Type: "Goods",
    Description: "Food supplies and clothing",
    Amount: 2500.00,
    Date: new Date("2024-01-16T14:30:00Z")
  },
  {
    Type: "Services",
    Description: "Medical services and counseling",
    Amount: 3000.00,
    Date: new Date("2024-01-17T09:15:00Z")
  }
]);

// Insert Beneficiaries
db.Beneficiaries.insertMany([
  {
    Name: "John Doe",
    Contact: "john.doe@example.com",
    Address: "123 Main Street, City, State 12345",
    ReceivedDonations: []
  },
  {
    Name: "Jane Smith",
    Contact: "jane.smith@example.com",
    Address: "456 Oak Avenue, City, State 67890",
    ReceivedDonations: []
  }
]);

// Insert Volunteers
db.Volunteers.insertMany([
  {
    Name: "Alice Johnson",
    Contact: "alice.johnson@example.com",
    AssignedDisasters: []
  },
  {
    Name: "Bob Williams",
    Contact: "bob.williams@example.com",
    AssignedDisasters: []
  }
]);

// Insert Disasters
db.Disasters.insertMany([
  {
    Name: "Hurricane Alpha",
    Type: "Hurricane",
    DateOccurred: new Date("2024-01-10T08:00:00Z"),
    Location: "Coastal Region, State"
  },
  {
    Name: "Flood Beta",
    Type: "Flood",
    DateOccurred: new Date("2024-01-12T12:00:00Z"),
    Location: "River Valley, State"
  }
]);

print("Sample data imported successfully!");
print("Collections:");
print("  Donations: " + db.Donations.countDocuments());
print("  Beneficiaries: " + db.Beneficiaries.countDocuments());
print("  Volunteers: " + db.Volunteers.countDocuments());
print("  Disasters: " + db.Disasters.countDocuments());
