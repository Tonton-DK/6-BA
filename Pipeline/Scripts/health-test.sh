#!/bin/bash 

MAX_ATTEMPTS=10
CURRENT_ATTEMPT=1

while [ $CURRENT_ATTEMPT -le $MAX_ATTEMPTS ]; do
    echo "Testing connection - Attempt: ${CURRENT_ATTEMPT} of ${MAX_ATTEMPTS}"
    RESULT=$(curl -o /dev/null -s -w "%{http_code}\n" http://localhost:8080/api/HealthCheck/)
    
    if [ $RESULT -ne '200' ]; then
     echo "Connection didn't return a status code of 200. Connection failed"
     exit 1
    fi

    CURRENT_ATTEMPT=$((CURRENT_ATTEMPT + 1))
    sleep 1s
done

echo "All connection attempts were successful"
exit 0