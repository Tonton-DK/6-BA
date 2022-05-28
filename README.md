# BA
Bachelor project

# Local Docker Run With Local Images
```cd /```

```docker compose build```

```docker compose up```

go to http://localhost:8080/

# Local Docker Run With External Images
```cd /```

```docker-compose -f docker-compose-github.yml up```

go to http://localhost:8080/

# Local Kubernetes Run With External Images
Install Docker Desktop + Kind

```cd /```

```kind create cluster```

```
kubectl apply -f Pipeline/Kubernetes/deploy-metric-server.yaml
kubectl apply -f Pipeline/Kubernetes/deploy-review.yml
kubectl apply -f Pipeline/Kubernetes/deploy-contract.yml
kubectl apply -f Pipeline/Kubernetes/deploy-offer.yml
kubectl apply -f Pipeline/Kubernetes/deploy-job.yml
kubectl apply -f Pipeline/Kubernetes/deploy-user.yml
kubectl apply -f Pipeline/Kubernetes/deploy-frontend.yml
```

```
kubectl autoscale deployment frontend-deployment --cpu-percent=50 --min=1 --max=10
kubectl autoscale deployment user-service-deployment --cpu-percent=50 --min=1 --max=10
kubectl autoscale deployment job-service-deployment --cpu-percent=50 --min=1 --max=10
kubectl autoscale deployment offer-service-deployment --cpu-percent=50 --min=1 --max=10
kubectl autoscale deployment contract-service-deployment --cpu-percent=50 --min=1 --max=10
kubectl autoscale deployment review-service-deployment --cpu-percent=50 --min=1 --max=10
```

```kubectl get hpa```

```kubectl port-forward services/frontend 80:80```

go to http://localhost:80/

```kind delete cluster```
