#!/bin/sh

echo "Running tests..."

# ALWAYS go to solution root
cd /source/src || exit 1

# Run tests explicitly on solution
dotnet test Kmums.sln --no-build --verbosity minimal

RESULT=$?

if [ $RESULT -eq 0 ]; then
  echo "RESULT: PASS"
else
  echo "RESULT: FAIL"
fi