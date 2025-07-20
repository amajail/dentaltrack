#!/bin/bash

# 🎯 GitHub Milestones Setup Script  
# This script creates all the required milestones for DentalTrack project
# Run with: bash .github/setup-milestones.sh

echo "🎯 Setting up GitHub milestones for DentalTrack..."

# Calculate dates (2-week intervals starting from today)
TODAY=$(date +%Y-%m-%d)
SPRINT0=$(date -d "+2 weeks" +%Y-%m-%d)
SPRINT1=$(date -d "+4 weeks" +%Y-%m-%d)
SPRINT2=$(date -d "+6 weeks" +%Y-%m-%d)
SPRINT3=$(date -d "+9 weeks" +%Y-%m-%d)
SPRINT4=$(date -d "+12 weeks" +%Y-%m-%d)
SPRINT5=$(date -d "+14 weeks" +%Y-%m-%d)
SPRINT6=$(date -d "+16 weeks" +%Y-%m-%d)
SPRINT7=$(date -d "+18 weeks" +%Y-%m-%d)

echo "Creating Sprint milestones..."

# Sprint 0 - Setup
gh api repos/:owner/:repo/milestones \
  --method POST \
  --field title="Sprint 0 - Setup" \
  --field description="Configuración inicial del monorepo y herramientas de desarrollo" \
  --field due_on="${SPRINT0}T23:59:59Z" \
  --field state="open"

# Sprint 1 - API & Auth  
gh api repos/:owner/:repo/milestones \
  --method POST \
  --field title="Sprint 1 - API & Auth" \
  --field description="API base y sistema de autenticación con Google OAuth" \
  --field due_on="${SPRINT1}T23:59:59Z" \
  --field state="open"

# Sprint 2 - Frontend & Patients
gh api repos/:owner/:repo/milestones \
  --method POST \
  --field title="Sprint 2 - Frontend & Patients" \
  --field description="Frontend base y gestión de pacientes" \
  --field due_on="${SPRINT2}T23:59:59Z" \
  --field state="open"

# Sprint 3 - Photos
gh api repos/:owner/:repo/milestones \
  --method POST \
  --field title="Sprint 3 - Photos" \
  --field description="Sistema de captura y almacenamiento de fotos" \
  --field due_on="${SPRINT3}T23:59:59Z" \
  --field state="open"

# Sprint 4 - AI
gh api repos/:owner/:repo/milestones \
  --method POST \
  --field title="Sprint 4 - AI" \
  --field description="Integración de IA para análisis de imágenes dentales" \
  --field due_on="${SPRINT4}T23:59:59Z" \
  --field state="open"

# Sprint 5 - Reports
gh api repos/:owner/:repo/milestones \
  --method POST \
  --field title="Sprint 5 - Reports" \
  --field description="Sistema de reportes y seguimiento" \
  --field due_on="${SPRINT5}T23:59:59Z" \
  --field state="open"

# Sprint 6 - Testing
gh api repos/:owner/:repo/milestones \
  --method POST \
  --field title="Sprint 6 - Testing" \
  --field description="Testing integral y optimización" \
  --field due_on="${SPRINT6}T23:59:59Z" \
  --field state="open"

# Sprint 7 - Deploy
gh api repos/:owner/:repo/milestones \
  --method POST \
  --field title="Sprint 7 - Deploy" \
  --field description="Deployment y lanzamiento del MVP" \
  --field due_on="${SPRINT7}T23:59:59Z" \
  --field state="open"

echo "✅ All milestones created successfully!"
echo "📅 Sprint timeline:"
echo "  Sprint 0: ${TODAY} → ${SPRINT0}"
echo "  Sprint 1: ${SPRINT0} → ${SPRINT1}"
echo "  Sprint 2: ${SPRINT1} → ${SPRINT2}"
echo "  Sprint 3: ${SPRINT2} → ${SPRINT3}"
echo "  Sprint 4: ${SPRINT3} → ${SPRINT4}"
echo "  Sprint 5: ${SPRINT4} → ${SPRINT5}"
echo "  Sprint 6: ${SPRINT5} → ${SPRINT6}"
echo "  Sprint 7: ${SPRINT6} → ${SPRINT7}"