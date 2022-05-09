kind create cluster
kubectl create secret docker-registry regcred --docker-server=ghcr.io --docker-username=antje12 --docker-password=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 --docker-email=antje12@student.sdu.dk
kubectl apply -f Pipeline/Kubernetes/deploy-metric-server.yaml
kubectl apply -f Pipeline/Kubernetes/deploy-review.yml
kubectl apply -f Pipeline/Kubernetes/deploy-contract.yml
kubectl apply -f Pipeline/Kubernetes/deploy-offer.yml
kubectl apply -f Pipeline/Kubernetes/deploy-job.yml
kubectl apply -f Pipeline/Kubernetes/deploy-user.yml
kubectl apply -f Pipeline/Kubernetes/deploy-frontend.yml