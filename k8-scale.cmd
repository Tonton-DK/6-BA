kubectl autoscale deployment frontend-deployment --cpu-percent=80 --min=1 --max=10
kubectl autoscale deployment user-service-deployment --cpu-percent=80 --min=1 --max=10
kubectl autoscale deployment job-service-deployment --cpu-percent=80 --min=1 --max=10
kubectl autoscale deployment offer-service-deployment --cpu-percent=80 --min=1 --max=10
kubectl autoscale deployment contract-service-deployment --cpu-percent=80 --min=1 --max=10
kubectl autoscale deployment review-service-deployment --cpu-percent=80 --min=1 --max=10