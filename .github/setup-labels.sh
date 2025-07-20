#!/bin/bash

# 🏷️ GitHub Labels Setup Script
# This script creates all the required labels for DentalTrack project
# Run with: bash .github/setup-labels.sh

echo "🏷️ Setting up GitHub labels for DentalTrack..."

# Priority Labels
echo "Creating Priority labels..."
gh label create "priority: critical" --color "d73027" --description "Critical priority issue"
gh label create "priority: high" --color "fc8d59" --description "High priority issue"  
gh label create "priority: medium" --color "fee08b" --description "Medium priority issue"
gh label create "priority: low" --color "91cf60" --description "Low priority issue"

# Type Labels
echo "Creating Type labels..."
gh label create "bug" --color "d73027" --description "Something isn't working"
gh label create "enhancement" --color "4575b4" --description "New feature or request"
gh label create "user-story" --color "6a4c93" --description "User story for development"
gh label create "documentation" --color "17a2b8" --description "Improvements or additions to documentation"
gh label create "medical" --color "e83e8c" --description "Medical or clinical functionality"

# Epic Labels  
echo "Creating Epic labels..."
gh label create "epic: setup" --color "6c757d" --description "Epic 1: Setup and configuration"
gh label create "epic: api" --color "007bff" --description "Epic 2: Backend Web API"
gh label create "epic: react" --color "17a2b8" --description "Epic 3: React Web App" 
gh label create "epic: auth" --color "ffc107" --description "Epic 6: Authentication"
gh label create "epic: patients" --color "28a745" --description "Epic 7: Patient management"
gh label create "epic: photos" --color "fd7e14" --description "Epic 8: Photo management"
gh label create "epic: ai" --color "6f42c1" --description "Epic 9: AI analysis"
gh label create "epic: reports" --color "e83e8c" --description "Epic 10: Reports and analytics"

# Status Labels
echo "Creating Status labels..."
gh label create "needs-triage" --color "ffc107" --description "Needs triage and prioritization"
gh label create "in-progress" --color "007bff" --description "Currently being worked on"
gh label create "needs-review" --color "fd7e14" --description "Needs code review"
gh label create "blocked" --color "dc3545" --description "Blocked by external dependency"

echo "✅ All labels created successfully!"
echo "📋 Next steps:"
echo "  1. Create GitHub Project manually"
echo "  2. Configure milestones"
echo "  3. Set up automation rules"
echo "  4. Configure repository settings"